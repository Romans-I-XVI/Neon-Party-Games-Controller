using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using NeonPartyGamesController.Entities.Buttons;
using NeonPartyGamesController.Enums;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities
{
	public class RokuIPButtonSpawner : Entity
	{
		public readonly int ScanDelay = 250;
		private bool UdpListenerRunning = false;
		private readonly Socket SendSocket;
		private readonly Socket ReceiveSocket;
		private readonly IPEndPoint RemoteEndPoint;
		private IPEndPoint IPEndPoint => new IPEndPoint(IPAddress.Parse(Settings.RokuSearchAddress), Settings.RokuSearchPort);
		private readonly GameTimeSpan ScanDelayTimer = new GameTimeSpan();
		private readonly float Scale;
		private readonly Vector2[] Positions = new Vector2[6];
		private readonly List<ButtonRokuIP> Buttons = new List<ButtonRokuIP>();

		public RokuIPButtonSpawner() {
			this.SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			this.ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			this.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Settings.RokuSearchAddress), Settings.RokuSearchPort);
			try {
				this.SendSocket.Connect(this.RemoteEndPoint);
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}
			try {
				this.ReceiveSocket.Bind(new IPEndPoint(((IPEndPoint)this.SendSocket.LocalEndPoint).Address, Settings.RokuSearchPort));
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}
			this.StartUdpListener();
			this.SendScanPacket();
			const float required_space = 1200;
			this.Scale = (Engine.Game.CanvasWidth < required_space) ? Engine.Game.CanvasWidth / required_space : 1f;

			int spread_x = (int)(400 * this.Scale);
			int spread_y = 200;
			int start_x = Engine.Game.CanvasWidth / 2;
			int start_y = 200;

			int i = 0;
			for (int y = 0; y < 2; y++) {
				int offset_y = y * spread_y;
				for (int x = 0; x < 3; x++) {
					int offset_x = (spread_x * x + spread_x / 2) - (spread_x * 3) / 2;
					this.Positions[i] = new Vector2(start_x + offset_x, start_y + offset_y);
					i++;
				}
			}
		}

		~RokuIPButtonSpawner() {
			this.IsExpired = true;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.ScanDelayTimer.TotalMilliseconds >= this.ScanDelay) {
				this.SendScanPacket();
				this.ScanDelayTimer.Mark();
			}
		}

		public override void onDestroy() {
			base.onDestroy();
			this.Dispose();
		}

		public override void onChangeRoom(Room previous_room, Room next_room) {
			base.onChangeRoom(previous_room, next_room);
			this.Dispose();
		}

		private void SendScanPacket() {
			try {
				this.SendSocket.SendTo(Settings.RokuSearchBytes, this.RemoteEndPoint);
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			}
		}

		private void StartUdpListener() {
//			if (!this.UdpListenerRunning)
//				Task.Run(this.UdpListenTask);
		}

		private async void UdpListenTask() {
			byte[] receive_buffer = new byte[2048];
			EndPoint ep = new IPEndPoint(IPAddress.Any, 0);

			this.UdpListenerRunning = true;
			while (this.Buttons.Count < this.Positions.Length && !this.IsExpired) {
				string name = null;
				string roku_ip = null;
				try {
					int received = 0;
					try {
						received = this.ReceiveSocket.ReceiveFrom(receive_buffer, ref ep);
					} catch (Exception e) {
						Debug.WriteLine(e);
					}
					if (this.IsExpired) break;
					if (received > 0) {
						string received_text = Encoding.ASCII.GetString(receive_buffer, 0, received);
						if (!string.IsNullOrEmpty(received_text)) {
							bool is_roku = received_text.ToLower().Contains("roku:ecp");
							if (is_roku) {
								roku_ip = ((IPEndPoint)ep).Address.ToString();
								name = await RokuECP.GetRokuName(roku_ip);
							}
							if (this.IsExpired) break;
						}
					}
				} catch {}

				if (roku_ip != null && name != null)
					this.SpawnButton(name, roku_ip);
			}

			this.UdpListenerRunning = false;
		}

		private void SpawnButton(string roku_name, string roku_ip) {
			lock (this.Buttons) {
				if (this.Buttons.Count < this.Positions.Length && !this.DoesButtonWithIPExist(roku_ip) && Engine.Room is RoomSelectRoku && !this.IsExpired) {
					int i = this.Buttons.Count;
					var pos = this.Positions[i].ToPoint();
					var button = new ButtonRokuIP(pos.X, pos.Y, this.Scale, roku_name, roku_ip);
					this.Buttons.Add(button);
					Engine.SpawnInstance(button);
				}
			}
		}

		private bool DoesButtonWithIPExist(string roku_ip) {
			foreach (var button in this.Buttons) {
				if (button.ButtonText[1] == roku_ip)
					return true;
			}

			return false;
		}

		private void Dispose() {
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
