using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class Trackpad : Entity
	{
		public const int DefaultWidth = 460;
		public const int DefaultHeight = (int)(460 / 1.77777777777);
		public const float DefaultScale = 0.575f;
		public float Scale;
		public bool InMoveMode = false;

		public Trackpad() {
			this.Position = Settings.TrackpadPosition;
			this.Scale = Settings.TrackpadScale;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			Region region;
			if (!this.InMoveMode)
				region = SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Extra.Trackpad);
			else
				region = SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Extra.TrackpadMoving);

			sprite_batch.Draw(region, this.Position, Color.White, 0, new Vector2(this.Scale));
		}
	}
}
