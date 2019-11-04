using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSelectFace : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.SelectFace));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -270 / 2, 360, 270);
		private static Action _on_click => () => Engine.ChangeRoom<RoomSelectFace>();

		public ButtonSelectFace(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, _on_click) {

		}
	}
}
