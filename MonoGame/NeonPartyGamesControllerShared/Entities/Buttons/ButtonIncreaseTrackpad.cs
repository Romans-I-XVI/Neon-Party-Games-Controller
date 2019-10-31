using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonIncreaseTrackpad : ButtonTrackpadControlBase
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.IncreaseTrackpad));
		private static Rectangle _collider_rect => new Rectangle(-100 / 2, -100 / 2, 100, 100);

		public ButtonIncreaseTrackpad(int x, int y, Trackpad trackpad) : base(x, y, 1, _sprite, _collider_rect, trackpad.IncreaseScale, trackpad) {
			this.OnClickHeld = trackpad.IncreaseScale;
		}
	}
}
