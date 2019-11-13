using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities
{
	public class PlayerNetworkingControl : Entity
	{
		public const int SendDataSize = 50;
		public const int ReceiveDataSize = 4;
		// Packets will send up to 4 times per target Roku frame (60fps)
		// This makes sure the Roku always has the latest data but without flooding the socket
		private const float MinimumSendDelay = 16.66666666f / 4f;
		public readonly Player Player;
		private readonly Socket SendSocket;
		private readonly Socket ReceiveSocket;
		private readonly IPEndPoint RemoteEndPoint;
		private byte[] Data;
		private readonly GameTimeSpan SendDelayTimer;
		private bool SentDestroyPacket = false;
		private readonly IPAddress RokuIP;
		private readonly int RokuPort;

		public PlayerNetworkingControl(Player player, IPAddress roku_ip, int roku_port) {
			this.Depth = player.Depth - 1;
			this.Player = player;
			this.RokuIP = roku_ip;
			this.RokuPort = roku_port;
			this.SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) {
				SendTimeout = 100
			};
			this.ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) {
				ReceiveTimeout = 100
			};
			this.RemoteEndPoint = new IPEndPoint(this.RokuIP, this.RokuPort);
			try {
				this.SendSocket.Connect(this.RemoteEndPoint);
			} catch {}
			try {
				this.ReceiveSocket.Bind(new IPEndPoint(this.GetMyIP(), this.RokuPort));
			} catch {}
			this.SendDelayTimer = new GameTimeSpan();
			this.InitializeData();
			this.StartSendingPackets();
			this.StartReceivingPackets();
			NeonPartyGamesControllerGame.FocusChangeEvent += this.onFocusChange;
		}

		~PlayerNetworkingControl() {
			this.IsExpired = true;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.UpdateData(this.Player.GetRokuScreenPosition());
		}

		public override void onDestroy() {
			base.onDestroy();
			this.Dispose();
		}

		public override void onChangeRoom(Room previous_room, Room next_room) {
			base.onChangeRoom(previous_room, next_room);
			this.Dispose();
		}

		public void onFocusChange(bool has_focus) {
			if (!has_focus)
				this.SendDestroyPacket();
			else
				this.SentDestroyPacket = false;
		}

		private void StartSendingPackets() {
			byte[] send_buffer = new byte[PlayerNetworkingControl.SendDataSize];

			Task.Run(() => {
				while (!this.IsExpired) {
					float current_time = this.SendDelayTimer.TotalMilliseconds;
					if (current_time < PlayerNetworkingControl.MinimumSendDelay) {
						int time_to_sleep = (int)(PlayerNetworkingControl.MinimumSendDelay - current_time);
						Thread.Sleep(time_to_sleep);
					}
					this.SendDelayTimer.Mark();

					lock (this.Data) {
						this.Data.CopyTo(send_buffer, 0);
					}

					if (!this.SentDestroyPacket) {
						try {
							this.SendSocket.SendTo(send_buffer, this.RemoteEndPoint);
						} catch {}
					}
				}
			});
		}

		private void StartReceivingPackets() {
			byte[] receive_buffer = new byte[PlayerNetworkingControl.ReceiveDataSize];
			EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
			Task.Run(() => {
				while (!this.IsExpired) {
					int received = 0;
					try {
						received = this.ReceiveSocket.ReceiveFrom(receive_buffer, ref ep);
					} catch {}

					if (received > 0 && ((IPEndPoint)ep).Address.Equals(this.RokuIP) && ((IPEndPoint)ep).Port == this.RokuPort && NeonPartyGamesControllerGame.HasFocus) {
						if (received >= 3)
							SoundPlayer.Play(receive_buffer[0], receive_buffer[1], receive_buffer[2]);
						if (received >= 4)
							HapticFeedbackPlayer.Play(receive_buffer[3]);
					}
				}
			});
		}

		private void SendDestroyPacket() {
			this.SentDestroyPacket = true;
			try {
				byte[] send_buffer = new byte[PlayerNetworkingControl.SendDataSize];
				lock (this.Data) {
					this.Data.CopyTo(send_buffer, 0);
				}

				send_buffer[10] = Convert.ToByte(true);
				this.SendSocket.SendTo(send_buffer, this.RemoteEndPoint);
			} catch {}
		}

		private IPAddress GetMyIP() {
			IPAddress address_from_socket = null;
			try {
				address_from_socket = ((IPEndPoint)this.SendSocket.LocalEndPoint)?.Address;
			} catch {}

			if (address_from_socket != null)
				return address_from_socket;

			List<IPAddress> addresses = new List<IPAddress>();

			// Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
			NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface network in networkInterfaces)
			{
				// Read the IP configuration for each network
				IPInterfaceProperties properties = network.GetIPProperties();

				if (properties.UnicastAddresses.Count > 0) {
					var address = properties.UnicastAddresses[0];

					// We're only interested in IPv4 addresses for now
					if (address.Address.AddressFamily != AddressFamily.InterNetwork)
						continue;

					// Ignore loopback addresses (e.g., 127.0.0.1)
					if (IPAddress.IsLoopback(address.Address))
						continue;

					addresses.Add(address.Address);
				}
			}

			if (addresses.Count == 1) {
				return addresses[0];
			} else if (addresses.Count > 1) {
				if (this.RokuIP == null)
					return null;

				var best_choice = addresses[0];
				for (int i = addresses.Count - 1; i >= 0; i--) {
					var local_bytes = addresses[i].GetAddressBytes();
					var remote_bytes = this.RokuIP.GetAddressBytes();
					if (local_bytes.Length == 4 &&
					    remote_bytes.Length == 4 &&
						local_bytes[0] == remote_bytes[0] &&
					    local_bytes[1] == remote_bytes[1] &&
					    local_bytes[2] == remote_bytes[2]) {
						best_choice = addresses[i];
					}
				}

				return best_choice;
			}

			return null;
		}

		private void InitializeData() {
			this.Data = new byte[PlayerNetworkingControl.SendDataSize];
			var ip = this.GetMyIP() ?? new IPAddress(new byte[4]);
			ushort id = Settings.ID;
			var pos = this.Player.GetRokuScreenPosition();

			byte fill = 0;
			byte[] ip_bytes = ip.GetAddressBytes();
			byte[] id_bytes = BitConverter.GetBytes(id);
			byte[] x_bytes = BitConverter.GetBytes(pos.X);
			byte[] y_bytes = BitConverter.GetBytes(pos.Y);
			byte destroy_byte = Convert.ToByte(false);
			byte face_byte = (byte)Settings.PlayerFace;
			byte color_byte = (byte)Settings.PlayerColor;
			byte name_length_byte = (byte)(uint)Settings.PlayerName.Length;
			byte[] name_bytes = Encoding.ASCII.GetBytes(Settings.PlayerName);

			this.Data[0] = ip_bytes[0];
			this.Data[1] = ip_bytes[1];
			this.Data[2] = ip_bytes[2];
			this.Data[3] = ip_bytes[3];
			this.Data[4] = id_bytes[0];
			this.Data[5] = id_bytes[1];
			this.Data[6] = x_bytes[0];
			this.Data[7] = x_bytes[1];
			this.Data[8] = y_bytes[0];
			this.Data[9] = y_bytes[1];
			this.Data[10] = destroy_byte;
			this.Data[11] = face_byte;
			this.Data[12] = color_byte;
			this.Data[13] = name_length_byte;
			for (int i = 0; i < this.Data.Length - 14; i++)
				this.Data[14 + i] = (i < name_bytes.Length) ? name_bytes[i] : fill;
		}

		private void UpdateData(Point position) {
			byte[] x_bytes = BitConverter.GetBytes(position.X);
			byte[] y_bytes = BitConverter.GetBytes(position.Y);
			lock (this.Data) {
				this.Data[6] = x_bytes[0];
				this.Data[7] = x_bytes[1];
				this.Data[8] = y_bytes[0];
				this.Data[9] = y_bytes[1];
			}
		}

		private void Dispose() {
			NeonPartyGamesControllerGame.FocusChangeEvent -= this.onFocusChange;
			if (!this.SentDestroyPacket)
				this.SendDestroyPacket();

			try {
				this.SendSocket.Close();
			} catch {}
			try {
				this.SendSocket.Dispose();
			} catch {}
            try {
                this.ReceiveSocket.Close();
            } catch {}
            try {
                this.ReceiveSocket.Dispose();
            } catch {}
		}
	}
}
