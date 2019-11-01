using System;
using System.Diagnostics;
using System.Net;
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
		public readonly Player Player;
		private readonly Socket Socket;
		private readonly IPEndPoint RemoteEndPoint;
		private byte[] Data;
		private readonly float MinimumSendDelay = 16.66666666f;
		private readonly GameTimeSpan SendDelayTimer;
		private ManualResetEvent SendWaitHandle = null;

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
				while (!this.IsExpired) {
					this.SendWaitHandle.WaitOne();
					this.SendWaitHandle.Reset();

					float current_time = this.SendDelayTimer.TotalMilliseconds;
					if (current_time < this.MinimumSendDelay) {
						int time_to_sleep = (int)(this.MinimumSendDelay - current_time);
						Thread.Sleep(time_to_sleep);
					}
					this.SendDelayTimer.Mark();

					lock (this.Data) {
						this.Data.CopyTo(send_buffer, 0);
					}
					try {
						this.Socket.SendTo(send_buffer, this.RemoteEndPoint);
					} catch {}
				}
			});
		}

		private IPAddress GetMyIP() {
			//TODO: Add proper retrieval of personal IP address
			return new IPAddress(new byte[] {192, 168, 1, 245});
		}

		private ushort GetMyID() {
			//TODO: Add proper retrieval of personal ID
			return 0;
		}

		private void InitializeData() {
			this.Data = new byte[PlayerNetworkingControl.SendDataSize];
			var ip = this.GetMyIP();
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
			try {
				this.Socket.Close();
			} catch {}
			try {
				this.Socket.Dispose();
			} catch {}
		}
	}
}
