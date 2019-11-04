using MonoEngine;
using NeonPartyGamesController.TexturePacker;

namespace NeonPartyGamesController
{
	public static class SpriteSheetHolder
	{
		public static SpriteSheet SpriteSheet { get; private set; } = null;

		public static void Init(params AvailableTextures[] sprite_sheet_textures) {
			SpriteSheet = new SpriteSheet();
			var sprite_sheet_loader = new SpriteSheetLoader();

			foreach (var available_texture in sprite_sheet_textures) {
				SpriteSheet.Add(sprite_sheet_loader.Load(ContentHolder.Get(available_texture), "NeonPartyGamesController." + Engine.Game.Content.RootDirectory + ".textures." + available_texture + ".json"));
			}
		}
	}
}
