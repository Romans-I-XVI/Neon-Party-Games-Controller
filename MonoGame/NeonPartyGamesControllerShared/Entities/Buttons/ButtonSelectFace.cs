using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSelectFace : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("buttons/select_face"));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -270 / 2, 360, 270);
		private static Action _on_click => () => Engine.ChangeRoom<RoomSelectFace>();

		public ButtonSelectFace(int x, int y) : base(x, y, _sprite, _collider_rect, _on_click) {

		}
	}
}
