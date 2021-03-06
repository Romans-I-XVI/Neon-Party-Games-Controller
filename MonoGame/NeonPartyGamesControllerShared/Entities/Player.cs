using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class Player : Entity, ITouchable
	{
		public readonly Trackpad Trackpad;
		private Vector2 DestPositionPercent;
		private Vector2 CurrentPositionPercent;
		private int CurrentTouchID;
		private bool IsCurrentClickValid;
		private bool IsLockedToInputPosition;
		private bool Active;

		public Player(Trackpad trackpad) {
			this.Trackpad = trackpad;
			this.Position = trackpad.Position;
			this.DestPositionPercent = new Vector2(0.5f, 0.5f);
			this.CurrentPositionPercent = new Vector2(0.5f, 0.5f);
			this.CurrentTouchID = -1;
			this.IsCurrentClickValid = false;
			this.IsLockedToInputPosition = true;
			this.Active = true;
			this.Depth = trackpad.Depth - 1;

			var sprite = new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Extra.Cursor)) {
				Scale = new Vector2(trackpad.Scale)
			};
			this.AddSprite("main", sprite);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			var sprite = this.GetSprite("main");
			sprite.Scale.X = this.Trackpad.Scale;
			sprite.Scale.Y = this.Trackpad.Scale;
			sprite.Enabled = !this.Trackpad.InMoveMode;
			this.Active = sprite.Enabled;
			if (this.Active) {
				this.UpdatePosition(dt);
			}
		}

		public override void onMouseDown(MouseEventArgs e) {
			base.onMouseDown(e);
			if (e.Button == MouseButtons.LeftButton) {
				this.onInputDown(e.Position);
			}
		}

		public override void onMouse(MouseState state) {
			base.onMouse(state);
			if (state.LeftButton == ButtonState.Pressed) {
				if (!this.IsCurrentClickValid)
					this.onInputDown(state.Position);

				this.onInput(state.Position);
			}
		}

		public void onTouchPressed(TouchLocation touch) {
			if (this.CurrentTouchID == -1) {
				this.onInputDown(touch.Position.ToPoint());
				if (this.IsCurrentClickValid) {
					this.CurrentTouchID = touch.Id;
				}
			}
		}

		public void onTouch(TouchCollection touch) {
			if (!this.IsCurrentClickValid) {
				for (int i = 0; i < touch.Count; i++) {
					this.onTouchPressed(touch[i]);
					if (this.IsCurrentClickValid)
						break;
				}
			}

			if (this.IsCurrentClickValid) {
				for (int i = 0; i < touch.Count; i++) {
					if (touch[i].Id == this.CurrentTouchID) {
						this.onInput(touch[i].Position.ToPoint());
						break;
					}
				}
			}
		}

		public void onTouchReleased(TouchLocation touch) {
			if (touch.Id == this.CurrentTouchID)
				this.CurrentTouchID = -1;
		}

		private void onInputDown(Point position) {
			if (!this.Active)
				return;

			var trackpad_rect = this.Trackpad.CurrentTrackpadRect;
			this.IsCurrentClickValid = CollisionChecking.Check(position, trackpad_rect);
			this.IsLockedToInputPosition = false;
		}

		private void onInput(Point position) {
			if (!this.Active)
				return;

			if (this.IsCurrentClickValid) {
				var dest_pos = this.ClampPositionToTrackpad(position.ToVector2());
				this.DestPositionPercent = this.CalculatePercentFromPosition(dest_pos);
			}
		}

		private void UpdatePosition(float dt) {
			if (this.IsLockedToInputPosition) {
				this.CurrentPositionPercent = this.DestPositionPercent;
			} else {
				float speed = 0.25f * 60 * dt;
				this.CurrentPositionPercent = GameUtilities.MoveTowards(this.CurrentPositionPercent, this.DestPositionPercent, speed);
				const float min_diff = 0.01f;
				float diff_x = Math.Abs(this.CurrentPositionPercent.X - this.DestPositionPercent.X);
				float diff_y = Math.Abs(this.CurrentPositionPercent.Y - this.DestPositionPercent.Y);
				if (diff_x <= min_diff && diff_y <= min_diff)
					this.IsLockedToInputPosition = true;
			}
			this.Position = this.CalculatePositionFromPercent(this.CurrentPositionPercent);
		}

		private Vector2 ClampPositionToTrackpad(Vector2 position) {
			var new_position = new Vector2(position.X, position.Y);

			var trackpad_rect = this.Trackpad.CurrentTrackpadRect;
			if (new_position.X < trackpad_rect.Left)
				new_position.X = trackpad_rect.Left;
			else if (new_position.X > trackpad_rect.Right)
				new_position.X = trackpad_rect.Right;

			if (new_position.Y < trackpad_rect.Top)
				new_position.Y = trackpad_rect.Top;
			else if (new_position.Y > trackpad_rect.Bottom)
				new_position.Y = trackpad_rect.Bottom;

			return new_position;
		}

		private Vector2 CalculatePositionFromPercent(Vector2 position_percent) {
			var trackpad_rect = this.Trackpad.CurrentTrackpadRect;

			float x = trackpad_rect.Left + trackpad_rect.Width * position_percent.X;
			float y = trackpad_rect.Top + trackpad_rect.Height * position_percent.Y;
			return new Vector2(x, y);
		}

		private Vector2 CalculatePercentFromPosition(Vector2 position) {
			var trackpad_rect = this.Trackpad.CurrentTrackpadRect;

			float normalized_x = position.X - trackpad_rect.Left;
			float normalized_y = position.Y - trackpad_rect.Top;
			float percent_x = normalized_x / trackpad_rect.Width;
			float percent_y = normalized_y / trackpad_rect.Height;
			return new Vector2(percent_x, percent_y);
		}

		public Point GetRokuScreenPosition() {
			int x = (int)(this.CurrentPositionPercent.X * Settings.RokuCanvasWidth);
			int y = (int)(this.CurrentPositionPercent.Y * Settings.RokuCanvasHeight);
			return new Point(x, y);
		}
	}
}
