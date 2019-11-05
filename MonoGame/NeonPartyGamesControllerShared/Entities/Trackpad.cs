using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController.Entities
{
	public class Trackpad : Entity, ITouchable
	{
		public bool InMoveMode { get; private set; }
		public const float BaseTrackpadWidth = 640;
		public const float BaseTrackpadHeight = 360;
		public const float MinimumBuffer = 50;
		public const float MinimumScale = 0.2f;
		public const float MaximumScale = 1.4f;
		public const float DefaultScale = 0.575f;
		public float Scale;
		public InputDevices? MovingWithDevice = null;
		public Point MoveModeStartOffset = Point.Zero;
		public int CurrentTouchID = -1;
		public float MaxX => Engine.Game.CanvasWidth - (Trackpad.BaseTrackpadWidth / 2) * this.Scale - Trackpad.MinimumBuffer;
		public float MaxY => Engine.Game.CanvasHeight - (Trackpad.BaseTrackpadHeight / 2) * this.Scale - Trackpad.MinimumBuffer;
		public float MinX => (Trackpad.BaseTrackpadWidth / 2) * this.Scale + Trackpad.MinimumBuffer;
		public float MinY => (Trackpad.BaseTrackpadHeight / 2) * this.Scale + Trackpad.MinimumBuffer;
		public int CurrentTrackpadWidth => (int)(Trackpad.BaseTrackpadWidth * this.Scale);
		public int CurrentTrackpadHeight => (int)(Trackpad.BaseTrackpadHeight * this.Scale);
		public Rectangle CurrentTrackpadRect => new Rectangle((int)(this.Position.X - this.CurrentTrackpadWidth / 2f), (int)(this.Position.Y - this.CurrentTrackpadHeight / 2f), this.CurrentTrackpadWidth, this.CurrentTrackpadHeight);

		public Trackpad() {
			if (Settings.TrackpadScale > 0) {
				this.Scale = Settings.TrackpadScale;
			} else {
				this.SetToDefaultScale();
			}

			if (Settings.TrackpadPosition.X > 0) {
				this.Position = Settings.TrackpadPosition;
			} else {
				this.SetToDefaultPosition();
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			string region_path = !this.InMoveMode ? AvailableRegions.Extra.Trackpad : AvailableRegions.Extra.TrackpadMoving;
			var region = SpriteSheetHolder.SpriteSheet.GetRegion(region_path);
			sprite_batch.Draw(region, this.Position, Color.White, 0, new Vector2(this.Scale));
		}

		public override void onMouseDown(MouseEventArgs e) {
			base.onMouseDown(e);
			if (this.InMoveMode && this.MovingWithDevice == null && CollisionChecking.Check(e.Position, this.CurrentTrackpadRect)) {
				this.MovingWithDevice = InputDevices.Mouse;
				this.MoveModeStartOffset = e.Position - this.CurrentTrackpadRect.Center;
			}
		}

		public override void onMouse(MouseState state) {
			base.onMouse(state);
			if (this.InMoveMode && this.MovingWithDevice == InputDevices.Mouse) {
				this.MoveTo((state.Position - this.MoveModeStartOffset).ToVector2());
			}
		}

		public override void onMouseUp(MouseEventArgs e) {
			base.onMouseUp(e);
			if (this.InMoveMode && this.MovingWithDevice == InputDevices.Mouse) {
				this.DisableMoveMode();
			}
		}

		public void onTouchPressed(TouchLocation touch) {
			if (this.InMoveMode && this.MovingWithDevice == null && CollisionChecking.Check(touch.Position, this.CurrentTrackpadRect)) {
				this.MovingWithDevice = InputDevices.Touch;
				this.MoveModeStartOffset = touch.Position.ToPoint() - this.CurrentTrackpadRect.Center;
				this.CurrentTouchID = touch.Id;
			}
		}

		public void onTouch(TouchCollection touch) {
			if (this.InMoveMode && this.MovingWithDevice == InputDevices.Touch) {
				for (int i = 0; i < touch.Count; i++) {
					if (touch[i].Id == this.CurrentTouchID) {
						this.MoveTo((touch[i].Position.ToPoint() - this.MoveModeStartOffset).ToVector2());
						break;
					}
				}
			}
		}

		public void onTouchReleased(TouchLocation touch) {
			if (this.InMoveMode && this.MovingWithDevice == InputDevices.Touch && touch.Id == this.CurrentTouchID) {
				this.DisableMoveMode();
			}
		}

		private void ClampSelfToScreen() {
			if (this.Position.X > this.MaxX)
				this.Position.X = this.MaxX;
			else if (this.Position.X < this.MinX)
				this.Position.X = this.MinX;

			if (this.Position.Y > this.MaxY)
				this.Position.Y = this.MaxY;
			else if (this.Position.Y < this.MinY)
				this.Position.Y = this.MinY;
		}

		private void SetToDefaultPosition() {
			this.Position = new Vector2(this.MaxX, this.MaxY);
			Settings.TrackpadPosition = this.Position;
		}

		private void SetToDefaultScale() {
			this.Scale = Trackpad.DefaultScale;
			Settings.TrackpadScale = this.Scale;
		}

		public void ResetPositionAndScale() {
			this.SetToDefaultScale();
			this.SetToDefaultPosition();
		}

		public void IncreaseScale() {
			this.Scale += 0.005f * 60 * Engine.Dt;
			if (this.Scale > Trackpad.MaximumScale) {
				this.Scale = Trackpad.MaximumScale;
			}
			this.ClampSelfToScreen();
		}

		public void ReduceScale() {
			this.Scale -= 0.005f * 60 * Engine.Dt;
			if (this.Scale < Trackpad.MinimumScale) {
				this.Scale = Trackpad.MinimumScale;
			}
			this.ClampSelfToScreen();
		}

		public void MoveTo(Vector2 position) {
			this.Position = position;
			this.ClampSelfToScreen();
		}

		public void EnableMoveMode() {
			this.MovingWithDevice = null;
			this.MoveModeStartOffset = Point.Zero;
			this.CurrentTouchID = -1;
			this.InMoveMode = true;
		}

		public void DisableMoveMode() {
			this.InMoveMode = false;
			Settings.TrackpadScale = this.Scale;
			Settings.TrackpadPosition = this.Position;
		}
	}
}
