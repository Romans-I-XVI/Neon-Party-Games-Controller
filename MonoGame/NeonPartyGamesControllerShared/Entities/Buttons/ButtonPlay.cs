using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonPlay : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("buttons/play"));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -270 / 2, 360, 270);
		private static Action _on_click => () => Engine.ChangeRoom<RoomPlay>();

		public ButtonPlay(int x, int y) : base(x, y, _sprite, _collider_rect, _on_click) {

		}
	}
}
