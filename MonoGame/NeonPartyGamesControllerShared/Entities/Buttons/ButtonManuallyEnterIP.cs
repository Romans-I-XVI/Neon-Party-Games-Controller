using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonManuallyEnterIP : Button
	{
		private readonly string[] ButtonText = {"Connect", "Manually"};
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.RokuIPSelect));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -170 / 2, 360, 170);
		private readonly float Scale;

		public ButtonManuallyEnterIP(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, ButtonManuallyEnterIP.ManuallyEnterIP) {
			this.Scale = scale;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			int spread = (int)(22 * this.Scale);
			for (int i = 0; i < this.ButtonText.Length; i++) {
				string text = this.ButtonText[i];
				var offset = new Vector2(0, spread * (i == 0 ? -1 : 1));
				sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.blippo), text, this.Position + offset, Color.White, 0, DrawFrom.Center, this.Scale, SpriteEffects.None, 0);
			}
		}

		private static void ManuallyEnterIP() {
			object[] args = null;
#if ANDROID
			args = new object[] {Android.Text.InputTypes.ClassPhone};
#endif
			PlatformFunctions.OpenInputDialog("Enter Roku IP Address", ButtonManuallyEnterIP.ParseIPText, args);
		}

		private static void ParseIPText(string ip_string) {
			bool success = IPAddress.TryParse(ip_string, out IPAddress ip) && ip_string.Split('.').Length == 4;
			if (success && ip != null) {
				Settings.RokuName = "";
				Settings.RokuIP = ip;
				Engine.ChangeRoom<RoomMain>();
			} else {
				PlatformFunctions.OpenMessageDialog("Invalid IP", ip_string + " is not a valid IP address");
			}
		}
	}
}
