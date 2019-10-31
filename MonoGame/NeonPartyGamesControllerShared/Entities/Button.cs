using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class Button : Entity, ITouchable
	{
		protected bool LastClickWasOnSelf { get; private set; }
		protected int LastClickTouchID { get; private set; }
		protected Action OnClick;
		protected Action OnClickHeld = null;

		private Button(int x, int y, float scale, Sprite sprite, Action on_click) {
			this.Position = new Vector2(x, y);
			sprite.Scale = new Vector2(scale);
			this.AddSprite("main", sprite);
			this.OnClick = on_click;
		}

		public Button(int x, int y, float scale, Sprite sprite, Rectangle collider_rect, Action on_click) : this(x, y, scale, sprite, on_click) {
			int offset_x = (int)(collider_rect.X * scale);
			int offset_y = (int)(collider_rect.Y * scale);
			int width = (int)(collider_rect.Width * scale);
			int height = (int)(collider_rect.Height * scale);
			this.AddColliderRectangle("main", offset_x, offset_y, width, height);
		}

		public Button(int x, int y, float scale, Sprite sprite, Circle collider_circle, Action on_click) : this(x, y, scale, sprite, on_click) {
			float radius = collider_circle.Radius * scale;
			int offset_x = (int)(collider_circle.X * scale);
			int offset_y = (int)(collider_circle.Y * scale);
			this.AddColliderCircle("main", radius, offset_x, offset_y);
		}

		public override void onMouseDown(MouseEventArgs e) {
			base.onMouseDown(e);
			if (this.IsClickOnSelf(e.Position)) {
				this.OnClick();
				this.LastClickWasOnSelf = true;
			} else {
				this.LastClickWasOnSelf = false;
			}
		}

		public override void onMouse(MouseState state) {
			base.onMouse(state);
			if (this.LastClickWasOnSelf && state.LeftButton == ButtonState.Pressed && this.IsClickOnSelf(state.Position)) {
				this.OnClickHeld?.Invoke();
			}
		}

		public void onTouchPressed(TouchLocation touch) {
			if (this.IsClickOnSelf(touch.Position.ToPoint())) {
				this.OnClick();
				this.LastClickWasOnSelf = true;
				this.LastClickTouchID = touch.Id;
			} else {
				this.LastClickWasOnSelf = false;
			}
		}

		public void onTouch(TouchCollection touch) {
			if (this.LastClickWasOnSelf) {
				for (int i = 0; i < touch.Count; i++) {
					if (touch[i].Id == this.LastClickTouchID) {
						if (IsClickOnSelf(touch[i].Position.ToPoint())) {
							this.OnClickHeld?.Invoke();
							break;
						}
					}
				}
			}
		}

		public void onTouchReleased(TouchLocation touch) {
		}

		private bool IsClickOnSelf(Point click_position) {
			var collider = this.GetCollider("main");
			switch (collider) {
			case ColliderRectangle rectangle:
				return CollisionChecking.Check(click_position, rectangle.Rectangle);
			case ColliderCircle circle:
				return CollisionChecking.Check(click_position, circle.Circle);
			default:
				return false;
			}
		}
	}
}
