using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class CurrentRokuDrawer : Entity
	{
		public const string DefaultText = "Please select your Roku";
		private readonly int MaxFontWidth = Engine.Game.CanvasWidth - 200;
		private readonly SpriteFont Font = ContentHolder.Get(AvailableFonts.blippo_small);

		public CurrentRokuDrawer() {
			this.Position = new Vector2(Engine.Game.CanvasWidth / 2, 50);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			float font_scale = 1f;
			string text = this.GetText();
			var text_measurement = this.Font.MeasureString(text);
			if (text_measurement.X > this.MaxFontWidth)
				font_scale = this.MaxFontWidth / text_measurement.X;
			sprite_batch.DrawString(this.Font, text, this.Position, Color.White, 0, DrawFrom.Center, font_scale, SpriteEffects.None, 0);
		}

		private string GetText() {
			if (Settings.RokuIP != null) {
				string text = (Settings.RokuName.Trim() != "") ? Settings.RokuName + " - " : "";
				text += Settings.RokuIP.ToString();
				return text;
			}

			return CurrentRokuDrawer.DefaultText;
		}
	}
}
