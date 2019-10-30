using System;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSetName : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.SetName));
		private static Rectangle _collider_rect => new Rectangle(-270 / 2, -140 / 2, 270, 140);

		public ButtonSetName(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, ButtonSetName.SetName) {

		}

		private static void SetName() {
#if Android
#elif IOS
#else
	#if DEBUG
			var debugger = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
			if (debugger != null) {
				Action<string> evaluator = s => {
					if (s != null && s.Trim() != "") {
						Settings.PlayerName = s.Trim();
					};
				};
				debugger.OpenConsoleWithCustomEvaluator(evaluator);
			}
	#endif
#endif
		}
	}
}
