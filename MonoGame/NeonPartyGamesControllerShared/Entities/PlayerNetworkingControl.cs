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
		// Packets will send up to 4 times per target Roku frame (60fps)
		// This makes sure the Roku always has the latest data but without flooding the socket
		private const float MinimumSendDelay = 16.66666666f / 4f;
		public readonly Player Player;
		private readonly Socket Socket;
		private readonly IPEndPoint RemoteEndPoint;
		private byte[] Data;
		private readonly GameTimeSpan SendDelayTimer;
		private ManualResetEvent SendWaitHandle = null;
		private bool SentDestroyPacket = false;

		public PlayerNetworkingControl(Player player, IPAddress roku_ip, int roku_port) {
			this.Depth = player.Depth - 1;
			this.Player = player;
			this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			this.SendDelayTimer = new GameTimeSpan();
			this.RemoteEndPoint = new IPEndPoint(roku_ip, roku_port);
			this.InitializeData();
			this.StartSendingPackets();
		}

		~PlayerNetworkingControl() {
			this.IsExpired = true;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.UpdateData(this.Player.GetRokuScreenPosition());
			this.SendWaitHandle.Set();
		}

		public override void onDestroy() {
			base.onDestroy();
			this.Dispose();
		}

		public override void onChangeRoom(Room previous_room, Room next_room) {
			base.onChangeRoom(previous_room, next_room);
			this.Dispose();
		}

		private void StartSendingPackets() {
			this.SendWaitHandle = new ManualResetEvent(true);
			byte[] send_buffer = new byte[PlayerNetworkingControl.SendDataSize];

			Task.Run(() => {
				while (!this.IsExpired && !this.SentDestroyPacket) {
					this.SendWaitHandle.WaitOne();
					this.SendWaitHandle.Reset();

					float current_time = this.SendDelayTimer.TotalMilliseconds;
					if (current_time < PlayerNetworkingControl.MinimumSendDelay) {
						int time_to_sleep = (int)(PlayerNetworkingControl.MinimumSendDelay - current_time);
						Thread.Sleep(time_to_sleep);
					}
					this.SendDelayTimer.Mark();

					lock (this.Data) {
						this.Data.CopyTo(send_buffer, 0);
					}

					// This extra check is for the rare scenario where the destroy packet gets sent between the while check and here.
					if (!this.SentDestroyPacket) {
						try {
							this.Socket.SendTo(send_buffer, this.RemoteEndPoint);
						} catch {}
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
				this.Socket.SendTo(send_buffer, this.RemoteEndPoint);
			} catch {}
		}

		private IPAddress GetMyIP() {
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
				if (Settings.RokuIP == null)
					return null;

				var best_choice = addresses[0];
				for (int i = addresses.Count - 1; i >= 0; i--) {
					var local_bytes = addresses[i].GetAddressBytes();
					var remote_bytes = Settings.RokuIP.GetAddressBytes();
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

		private ushort GetMyID() {
			//TODO: Add proper retrieval of personal ID
			return (ushort)Engine.Random.Next(ushort.MaxValue);
		}

		private void InitializeData() {
			this.Data = new byte[PlayerNetworkingControl.SendDataSize];
			var ip = this.GetMyIP() ?? new IPAddress(new byte[4]);
			ushort id = this.GetMyID();
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
			if (!this.SentDestroyPacket)
				this.SendDestroyPacket();

			try {
				this.Socket.Close();
			} catch {}
			try {
				this.Socket.Dispose();
			} catch {}
		}
	}
}
