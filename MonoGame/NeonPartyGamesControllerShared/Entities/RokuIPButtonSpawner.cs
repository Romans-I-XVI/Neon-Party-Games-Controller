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
		public readonly int ScanDelay = 50;
		private UdpClient UdpClient;
		private IPEndPoint IPEndPoint => new IPEndPoint(IPAddress.Parse(Settings.RokuSearchAddress), Settings.RokuSearchPort);
		private readonly GameTimeSpan ScanDelayTimer = new GameTimeSpan();
		private readonly float Scale;
		private readonly Vector2[] Positions = new Vector2[6];
		private readonly List<ButtonRokuIP> Buttons = new List<ButtonRokuIP>();

		public RokuIPButtonSpawner() {
			this.UdpClient = new UdpClient(Settings.RokuSearchPort, AddressFamily.InterNetwork);
			this.StartUdpListener();
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

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.ScanDelayTimer.TotalMilliseconds >= this.ScanDelay) {
				this.SendScanPacket();
				this.ScanDelayTimer.Mark();
			}
		}

		public override void onDestroy() {
			base.onDestroy();
			this.UdpClient.Close();
			this.UdpClient.Dispose();
			this.UdpClient = null;
		}

		public override void onChangeRoom(Room previousRoom, Room nextRoom) {
			base.onChangeRoom(previousRoom, nextRoom);
			this.UdpClient.Close();
			this.UdpClient.Dispose();
			this.UdpClient = null;
		}

		private void SendScanPacket() {
			this.UdpClient.SendAsync(Settings.RokuSearchBytes, Settings.RokuSearchBytes.Length, IPEndPoint);
		}

		private void StartUdpListener() {
			Task.Run(this.UdpListenTask);
		}

		private async void UdpListenTask() {
			while (this.Buttons.Count < this.Positions.Length && !this.IsExpired && this.UdpClient != null) {
				string name = null;
				string roku_ip = null;
				try {
					var ep = this.IPEndPoint;
					this.UdpClient.Receive(ref ep);
					if (this.IsExpired) break;
					roku_ip = ep.Address.ToString();
					name = await this.GetRokuName(roku_ip);
					if (this.IsExpired) break;
				} catch (Exception e) {
					Console.WriteLine(e);
				}

				if (roku_ip != null && name != null)
					this.SpawnButton(name, roku_ip);
			}
		}

		private async Task<string> GetRokuName(string roku_ip) {
			string name = "";

			var client = new HttpClient();
			byte[] response = await client.GetByteArrayAsync("http://" + roku_ip + ":8060/");
			string res = Encoding.ASCII.GetString(response, 0, response.Length - 1);

			try {
				int p_from = res.IndexOf("<friendlyName>") + "<friendlyName>".Length;
				int p_to = res.LastIndexOf("</friendlyName>");
				name = res.Substring(p_from, p_to - p_from);
			} catch {}

			if (name.Trim() == "") {
				try {
					int p_from = res.IndexOf("<modelName>") + "<modelName>".Length;
					int p_to = res.LastIndexOf("</modelName>");
					name = res.Substring(p_from, p_to - p_from);
				} catch {}
			}

			return HttpUtility.HtmlDecode(name);
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
	}
}
