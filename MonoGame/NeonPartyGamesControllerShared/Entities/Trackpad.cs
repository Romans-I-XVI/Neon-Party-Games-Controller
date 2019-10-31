using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class Trackpad : Entity
	{
		public const float BaseTrackpadWidth = 640;
		public const float BaseTrackpadHeight = 360;
		public const float MinimumBuffer = 40;
		public const float MinimumScale = 0.1f;
		public const float DefaultScale = 0.575f;
		public float Scale;
		public bool InMoveMode = false;
		public float MaxX => Engine.Game.CanvasWidth - (Trackpad.BaseTrackpadWidth / 2) * this.Scale - Trackpad.MinimumBuffer;
		public float MaxY => Engine.Game.CanvasHeight - (Trackpad.BaseTrackpadHeight / 2) * this.Scale - Trackpad.MinimumBuffer;
		public float MinX => (Trackpad.BaseTrackpadWidth / 2) * this.Scale + Trackpad.MinimumBuffer;
		public float MinY => (Trackpad.BaseTrackpadHeight / 2) * this.Scale + Trackpad.MinimumBuffer;
		public int CurrentTrackpadWidth => (int)(Trackpad.BaseTrackpadWidth * this.Scale);
		public int CurrentTrackpadHeight => (int)(Trackpad.BaseTrackpadHeight * this.Scale);
		public Rectangle CurrentTrackpadRect => new Rectangle((int)(this.Position.X - this.CurrentTrackpadWidth / 2f), (int)(this.Position.Y - this.CurrentTrackpadHeight / 2f), this.CurrentTrackpadWidth, this.CurrentTrackpadHeight);

		public Trackpad() {
			if (Settings.TrackpadScale > Trackpad.MinimumScale) {
				this.Scale = Settings.TrackpadScale;
			} else {
				this.Scale = Trackpad.DefaultScale;
				Settings.TrackpadScale = this.Scale;
			}

			if (Settings.TrackpadPosition.X > 0) {
				this.Position = Settings.TrackpadPosition;
			} else {
				this.Position = new Vector2(this.MaxX, this.MaxY);
				Settings.TrackpadPosition = this.Position;
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			string region_path = !this.InMoveMode ? AvailableRegions.Extra.Trackpad : AvailableRegions.Extra.TrackpadMoving;
			var region = SpriteSheetHolder.SpriteSheet.GetRegion(region_path);
			sprite_batch.Draw(region, this.Position, Color.White, 0, new Vector2(this.Scale));
		}
	}
}
