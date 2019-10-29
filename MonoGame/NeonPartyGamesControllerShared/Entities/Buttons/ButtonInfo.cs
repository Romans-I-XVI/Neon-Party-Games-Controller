using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonInfo : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion("buttons/info"));
		private static Circle _collider_circle => new Circle(-50, 50, 50);
		private static Action _on_click => () => Engine.ChangeRoom<RoomInfo>();

		public ButtonInfo(int x, int y) : base(x, y, 1, _sprite, _collider_circle, _on_click) {

		}
	}
}
