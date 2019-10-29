using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using NeonPartyGamesController.Enums;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonFace : Button
	{
		private static Circle _collider_circle => new Circle(0, 0, 60);
		private readonly Faces Face;
		private Colors CurrentColor;

		public ButtonFace(Faces face, int x, int y, float scale) : base(x, y, scale, ButtonFace.GetSprite(face), _collider_circle, ButtonFace.GetOnClick(face)) {
			this.Face = face;
			this.CurrentColor = Settings.PlayerColor;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.CurrentColor != Settings.PlayerColor) {
				var original_sprite_scale = this.GetSprite("main").Scale;
				this.RemoveSprite("main");
				var sprite = ButtonFace.GetSprite(this.Face);
				sprite.Scale = original_sprite_scale;
				this.AddSprite("main", sprite);
				this.CurrentColor = Settings.PlayerColor;
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			var region_pupils = SpriteSheetHolder.SpriteSheet.GetRegion("faces/character_pupils_" + (int)Settings.PlayerColor);
			sprite_batch.Draw(region_pupils, this.Position, Color.White, 0, this.GetSprite("main").Scale);
			base.onDraw(sprite_batch);
		}

		private static Sprite GetSprite(Faces face) {
			var region = SpriteSheetHolder.SpriteSheet.GetRegion("faces/character_" + (int)face + "_" + (int)Settings.PlayerColor);
			return new Sprite(region);
		}

		private static Action GetOnClick(Faces face) {
			return () => Settings.PlayerFace = face;
		}
	}
}
