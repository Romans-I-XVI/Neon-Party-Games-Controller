using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoEngine;

namespace NeonPartyGamesController.Entities
{
	public class Button : Entity, ITouchable
	{
		protected Action OnClick;

		private Button(int x, int y, Sprite sprite, Action on_click) {
			this.Position = new Vector2(x, y);
			this.AddSprite("main", sprite);
			this.OnClick = on_click;
		}

		public Button(int x, int y, Sprite sprite, Rectangle collider_rect, Action on_click) : this(x, y, sprite, on_click) {
			this.AddColliderRectangle("main", collider_rect.X, collider_rect.Y, collider_rect.Width, collider_rect.Height);
		}

		public Button(int x, int y, Sprite sprite, Circle collider_circle, Action on_click) : this(x, y, sprite, on_click) {
			this.AddColliderCircle("main", collider_circle.Radius, collider_circle.X, collider_circle.Y);
		}

		public override void onMouseDown(MouseEventArgs e) {
			base.onMouseDown(e);
			if (this.IsClickOnSelf(e.Position))
				this.OnClick();
		}

		public void onTouchPressed(TouchLocation touch) {
			if (this.IsClickOnSelf(touch.Position.ToPoint()))
				this.OnClick();
		}

		public void onTouch(TouchCollection touch) {
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
