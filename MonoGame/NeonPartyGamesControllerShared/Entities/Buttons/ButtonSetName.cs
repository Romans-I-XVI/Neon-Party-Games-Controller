using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSetName : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("buttons/set_name"));
		private static Rectangle _collider_rect => new Rectangle(-270 / 2, -140 / 2, 270, 140);

		public ButtonSetName(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, ButtonSetName.SetName) {

		}

		public static void SetName() {
			string text = "";
#if Android
#elif IOS
#else
			text = "Input Not Available";
#endif

			Settings.PlayerName = text;
		}
	}
}
