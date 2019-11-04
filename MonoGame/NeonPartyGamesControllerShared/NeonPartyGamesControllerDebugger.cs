using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace NeonPartyGamesController
{
	public class NeonPartyGamesControllerDebugger : DebuggerWithTerminal
	{
		public NeonPartyGamesControllerDebugger() : base(ContentHolder.Get(AvailableFonts.blippo)) {}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (!this.ConsoleOpen) {
				if (e.Key == Keys.D0)
					this.SetDrawColliders(Debugger.Variables["draw_colliders"] == "0");
			}
		}
	}
}
