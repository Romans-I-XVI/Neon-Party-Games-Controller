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
		}

		private void SpawnCreditsImage() {
			var credits = new EmptyEntity {
				Depth = 100,
				Position = new Vector2(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight / 2)
			};
			credits.AddSprite("main", new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Extra.Credits)));
			Engine.SpawnInstance(credits);
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
