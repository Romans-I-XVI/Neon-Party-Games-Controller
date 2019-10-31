using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonBackAvoidTrackpad : ButtonBack
	{
		private readonly int SwapPoint = 150;
		private readonly float TopY;
		private readonly float BottomY;
		private Trackpad Trackpad;

		public ButtonBackAvoidTrackpad(Trackpad trackpad) {
			this.Trackpad = trackpad;
			this.TopY = this.Position.Y;
			this.BottomY = Engine.Game.CanvasHeight - this.TopY - 100;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			this.Position.Y = this.Trackpad.CurrentTrackpadRect.Top < this.SwapPoint ? this.BottomY : this.TopY;
		}
	}
}
