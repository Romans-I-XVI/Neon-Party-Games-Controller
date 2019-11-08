using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSetName : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.SetName));
		private static Rectangle _collider_rect => new Rectangle(-270 / 2, -140 / 2, 270, 140);

		public ButtonSetName(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, ButtonSetName.SetName) {

		}

		private static void SetName() {
			PlatformFunctions.OpenInputDialog("Enter Name", result => Settings.PlayerName = result.Trim());
		}
	}
}
