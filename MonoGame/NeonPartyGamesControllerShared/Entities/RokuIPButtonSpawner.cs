using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using NeonPartyGamesController.Entities.Buttons;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController.Entities
{
	public class RokuIPButtonSpawner : Entity
	{
		public readonly int ScanDelay = 50;
		private readonly GameTimeSpan ScanDelayTimer = new GameTimeSpan();
		private readonly float Scale;
		private readonly Vector2[] Positions = new Vector2[6];
		private readonly List<ButtonRokuIP> Buttons = new List<ButtonRokuIP>();

		public RokuIPButtonSpawner() {
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

		private void SendScanPacket() {
			// TODO: Add code to send packet to scan for Rokus
		}

		private void SpawnButton(string roku_name, string roku_ip) {
			if (this.Buttons.Count < this.Positions.Length) {
				int i = this.Buttons.Count;
				var pos = this.Positions[i].ToPoint();
				var button = new ButtonRokuIP(pos.X, pos.Y, this.Scale, roku_name, roku_ip);
				this.Buttons.Add(button);
				Engine.SpawnInstance(button);
			}
		}
	}
}
