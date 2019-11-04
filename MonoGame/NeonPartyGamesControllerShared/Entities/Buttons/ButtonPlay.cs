using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonPlay : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.Play));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -270 / 2, 360, 270);
		private static Action _on_click => () => Engine.ChangeRoom<RoomPlay>();

		public ButtonPlay(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, _on_click) {

		}
	}
}
