using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonMoveTrackpad : ButtonTrackpadControlBase
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.MoveTrackpad));
		private static Rectangle _collider_rect => new Rectangle(-100 / 2, -100 / 2, 100, 100);

		public ButtonMoveTrackpad(int x, int y, Trackpad trackpad) : base(x, y, 1, _sprite, _collider_rect, GetOnClickAction(trackpad), trackpad) {

		}

		private static Action GetOnClickAction(Trackpad trackpad) {
			return () => {
				if (!trackpad.InMoveMode)
					trackpad.EnableMoveMode();
				else
					trackpad.DisableMoveMode();
			};
		}
	}
}
