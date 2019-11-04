using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonColor : Button
	{
		private static Rectangle _collider_rect => new Rectangle(-100 / 2, -100 / 2, 100, 100);

		public ButtonColor(Colors color, int x, int y, float scale) : base(x, y, scale, GetSprite(color), _collider_rect, GetOnClick(color)) {}

		private static Sprite GetSprite(Colors color) {
			var region = SpriteSheetHolder.SpriteSheet.GetRegion("buttons/color_" + (int)color);
			return new Sprite(region);
		}

		private static Action GetOnClick(Colors color) {
			return () => Settings.PlayerColor = color;
		}
	}
}
