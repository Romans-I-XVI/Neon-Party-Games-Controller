using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonTrackpadControlBase : Button
	{
		private readonly int SwapPoint = 150;
		private readonly float TopY;
		private readonly float BottomY;
		private readonly Trackpad Trackpad;

		public ButtonTrackpadControlBase(int x, int y, float scale, Sprite sprite, Rectangle collider_rect, Action action, Trackpad trackpad) : base(x, y, scale, sprite, collider_rect, action) {
			this.Trackpad = trackpad;
			this.TopY = this.Position.Y;
			this.BottomY = Engine.Game.CanvasHeight - this.TopY;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.Position.Y = this.Trackpad.CurrentTrackpadRect.Top < this.SwapPoint ? this.BottomY : this.TopY;
		}
	}
}
