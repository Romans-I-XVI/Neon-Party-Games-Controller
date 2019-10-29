using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class CurrentRokuDrawer : Entity
	{
		public const string DefaultText = "Please select your Roku";
		private readonly SpriteFont Font = ContentHolder.Get(AvailableFonts.blippo_small);

		public CurrentRokuDrawer() {
			this.Position = new Vector2(Engine.Game.CanvasWidth / 2, 50);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			sprite_batch.DrawString(this.Font, this.GetText(), this.Position, Color.White, 0, DrawFrom.Center, 1, SpriteEffects.None, 0);
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
