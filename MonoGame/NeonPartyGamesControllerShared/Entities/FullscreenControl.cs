using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class FullscreenControl : Entity
	{
		public FullscreenControl() {
			this.IsPersistent = true;
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.F11)
				Engine.Game.Graphics.ToggleFullScreen();
		}
	}
}
