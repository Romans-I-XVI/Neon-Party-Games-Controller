using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSelectRoku : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("buttons/select_roku"));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -270 / 2, 360, 270);
		private static Action _on_click => () => Engine.ChangeRoom<RoomSelectRoku>();

		public ButtonSelectRoku(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, _on_click) {

		}

	}
}
