using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class CurrentFaceDrawer : Entity
	{
		private readonly Vector2 SpriteScale = new Vector2(1.75f);
		private readonly SpriteFont Font = ContentHolder.Get(AvailableFonts.blippo_small);

		public CurrentFaceDrawer() {
			this.Position.X = Engine.Game.CanvasWidth / 2;
			this.Position.Y = 182;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			var region_face = SpriteSheetHolder.SpriteSheet.GetRegion("faces/character_" + (int)Settings.PlayerFace + "_" + (int)Settings.PlayerColor);
			var region_pupils = SpriteSheetHolder.SpriteSheet.GetRegion("faces/character_pupils_" + (int)Settings.PlayerColor);
			sprite_batch.Draw(region_face, this.Position, Color.White, 0, this.SpriteScale);
			sprite_batch.Draw(region_pupils, this.Position, Color.White, 0, this.SpriteScale);

			var offset = new Vector2(0, 100);
			sprite_batch.DrawString(this.Font, Settings.PlayerName, this.Position + offset, Color.White, 0, DrawFrom.TopCenter, 1f, SpriteEffects.None, 0);
		}
	}
}
