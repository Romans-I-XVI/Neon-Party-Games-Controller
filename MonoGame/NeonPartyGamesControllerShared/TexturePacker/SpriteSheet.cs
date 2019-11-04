using System.Collections.Generic;
using MonoEngine;

namespace NeonPartyGamesController.TexturePacker
{
	public class SpriteSheet
	{
		public SpriteSheet() {
			this.RegionList = new Dictionary<string, Region>();
		}

		public IDictionary<string, Region> RegionList { get; }

		public void Add(string name, Region region) {
			this.RegionList.Add(name, region);
		}

		public void Add(SpriteSheet other_sheet) {
			foreach (var region in other_sheet.RegionList)
				this.RegionList.Add(region);
		}

		public Region GetRegion(string name) {
			return this.RegionList[name];
		}

		public Region[] GetAnimation(string name) {
			var animations = new List<Region>();

			int i = 0;
			if (!this.RegionList.ContainsKey(name + "-" + i))
				i++;

			while (this.RegionList.ContainsKey(name + "-" + i)) {
				animations.Add(this.RegionList[name + "-" + i]);
				i++;
			}

			return animations.ToArray();
		}
	}
}
