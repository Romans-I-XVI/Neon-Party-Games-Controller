using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonHyperlink : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.Hyperlink));
		private static Circle _collider_circle => new Circle(0, 0, 50);

		public ButtonHyperlink(int x, int y, string link) : base(x, y, 1, _sprite, _collider_circle, ButtonHyperlink.GetOnClickAction(link)) {

		}

		private static Action GetOnClickAction(string link) {
			// TODO: Add hyperlink opening functionality
			return () => {};
		}

	}
}
