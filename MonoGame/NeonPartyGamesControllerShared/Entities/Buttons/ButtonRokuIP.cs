using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonRokuIP : Button
	{
		public readonly string[] ButtonText;
		private const int MaxFontWidth = 270;
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.RokuIPSelect));
		private static Rectangle _collider_rect => new Rectangle(-360 / 2, -170 / 2, 360, 170);
		private readonly float Scale;

		public ButtonRokuIP(int x, int y, float scale, string roku_name, string roku_ip) : base(x, y, scale, _sprite, _collider_rect, ButtonRokuIP.GetOnClickAction(roku_name, roku_ip)) {
			this.Scale = scale;
			this.ButtonText = new[] {roku_name, roku_ip};
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (!(Engine.Room is RoomSelectRoku)) {
				this.IsExpired = true;
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			var font = ContentHolder.Get(AvailableFonts.blippo);
			base.onDraw(sprite_batch);
			int spread = (int)(22 * this.Scale);
			for (int i = 0; i < this.ButtonText.Length; i++) {
				string text = this.ButtonText[i];
				float line_scale = 1.0f;
				var text_size = font.MeasureString(text);
				if (text_size.X > ButtonRokuIP.MaxFontWidth) {
					line_scale = ButtonRokuIP.MaxFontWidth / text_size.X;
				}
				var offset = new Vector2(0, spread * (i == 0 ? -1 : 1));
				sprite_batch.DrawString(font, text, this.Position + offset, Color.White, 0, DrawFrom.Center, this.Scale * line_scale, SpriteEffects.None, 0);
			}
		}

		private static Action GetOnClickAction(string roku_name, string ip_string) {
			return () => {
				bool success = IPAddress.TryParse(ip_string, out IPAddress ip);
				if (success && ip != null) {
					Settings.RokuName = roku_name != null ? roku_name.Trim() : "";
					Settings.RokuIP = ip;
					Engine.ChangeRoom<RoomMain>();
				}
			};
		}
	}
}
