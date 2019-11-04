using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonReduceTrackpad : ButtonTrackpadControlBase
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.ReduceTrackpad));
		private static Rectangle _collider_rect => new Rectangle(-100 / 2, -100 / 2, 100, 100);

		public ButtonReduceTrackpad(int x, int y, Trackpad trackpad) : base(x, y, 1, _sprite, _collider_rect, trackpad.ReduceScale, trackpad) {
			this.OnClickHeld = trackpad.ReduceScale;
			this.OnClickReleased = () => {
				Settings.TrackpadScale = trackpad.Scale;
				Settings.TrackpadPosition = trackpad.Position;
			};
		}
	}
}
