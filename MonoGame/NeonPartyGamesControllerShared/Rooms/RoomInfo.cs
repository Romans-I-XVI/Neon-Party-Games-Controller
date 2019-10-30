using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomInfo : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<ButtonBack>();
			this.SpawnCreditsImage();
			this.SpawnHyperlinks();
		}

		private void SpawnCreditsImage() {
			var credits = new EmptyEntity {
				Depth = 100,
				Position = new Vector2(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight / 2)
			};
			credits.AddSprite("main", new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Extra.Credits)));
			Engine.SpawnInstance(credits);
		}

		private void SpawnHyperlinks() {
			int start_x = Engine.Game.CanvasWidth / 2 + 360;
			const int start_y = 232;
			const int spread = 58;

			for (int i = 0; i < Settings.MusicLinks.Length; i++) {
				var button = new ButtonHyperlink(start_x, start_y + spread * i, Settings.MusicLinks[i]);
				Engine.SpawnInstance(button);
			}
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
