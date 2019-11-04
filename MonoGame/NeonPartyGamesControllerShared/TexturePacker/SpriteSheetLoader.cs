using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using Newtonsoft.Json;

namespace NeonPartyGamesController.TexturePacker
{
	public class SpriteSheetLoader
	{
		public SpriteSheet Load(Texture2D texture, string data_path) {
			var sheet = new SpriteSheet();
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream(data_path)) {
				if (stream == null)
					throw new NullReferenceException("stream is null, possible invalid data path");
				using (var reader = new StreamReader(stream)) {
					string result = reader.ReadToEnd();
					var spritesheet = JsonConvert.DeserializeObject<SpriteSheetJSON>(result);
					foreach (var kv in spritesheet.Frames) {
						string name = kv.Key;
						var item = kv.Value;
						var source_rectangle = new Rectangle(item.frame["x"], item.frame["y"], item.frame["w"], item.frame["h"]);
						var origin = new Vector2(0, 0);
						if (item.pivot != null) {
							origin.X = item.sourceSize["w"] * item.pivot["x"] - item.spriteSourceSize["x"];
							origin.Y = item.sourceSize["h"] * item.pivot["y"] - item.spriteSourceSize["y"];
						}

						var region = new Region(texture, source_rectangle, origin);
						sheet.Add(name, region);
					}
				}
			}

			return sheet;
		}

		// ReSharper disable InconsistentNaming
		public class SpriteSheetJSONFrame
		{
			[JsonProperty(PropertyName = "frame")] public Dictionary<string, int> frame;

			[JsonProperty(PropertyName = "pivot")] public Dictionary<string, float> pivot;

			[JsonProperty(PropertyName = "rotated")]
			public bool rotated;

			[JsonProperty(PropertyName = "sourceSize")]
			public Dictionary<string, int> sourceSize;

			[JsonProperty(PropertyName = "spriteSourceSize")]
			public Dictionary<string, int> spriteSourceSize;

			[JsonProperty(PropertyName = "trimmed")]
			public bool trimmed;
		}

		public class SpriteSheetJSONMeta
		{
			[JsonProperty(PropertyName = "app")] public string app;

			[JsonProperty(PropertyName = "format")]
			public string format;

			[JsonProperty(PropertyName = "image")] public string image;

			[JsonProperty(PropertyName = "scale")] public string scale;

			[JsonProperty(PropertyName = "size")] public Dictionary<string, int> size;

			[JsonProperty(PropertyName = "smartupdate")]
			public string smartupdate;

			[JsonProperty(PropertyName = "version")]
			public string version;
		}

		public class SpriteSheetJSON
		{
			[JsonProperty(PropertyName = "frames")]
			public Dictionary<string, SpriteSheetJSONFrame> Frames;

			[JsonProperty(PropertyName = "meta")] public SpriteSheetJSONMeta Meta;
		}
		// ReSharper restore InconsistentNaming
	}
}
