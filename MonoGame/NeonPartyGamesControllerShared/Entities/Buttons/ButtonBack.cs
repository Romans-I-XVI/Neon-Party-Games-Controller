using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonBack : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.Back));
		private static Circle _collider_circle => new Circle(50, 50, 50);
		private static Action _on_click => () => Engine.ChangeRoom<RoomMain>();

		public ButtonBack() : base(0, 0, 1, _sprite, _collider_circle, _on_click) {

		}
	}
}
