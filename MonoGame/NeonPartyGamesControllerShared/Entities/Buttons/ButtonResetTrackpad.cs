using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonResetTrackpad : ButtonTrackpadControlBase
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.ResetTrackpad));
		private static Rectangle _collider_rect => new Rectangle(-100 / 2, -100 / 2, 100, 100);

		public ButtonResetTrackpad(int x, int y, Trackpad trackpad) : base(x, y, 1, _sprite, _collider_rect, trackpad.ResetPositionAndScale, trackpad) {

		}
	}
}
