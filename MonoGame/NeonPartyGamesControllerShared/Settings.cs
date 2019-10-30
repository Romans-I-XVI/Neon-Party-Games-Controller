using System.Collections.Generic;
using System.Net;
using System.Text;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController
{
	public static class Settings
	{
		public const int MaxNameLength = 25;

		private static string _player_name = "Mr. Null";
		public static string PlayerName {
			get => Settings._player_name;
			set {
				value = value.Trim();
				if (value.Length > Settings.MaxNameLength) {
					value = value.Substring(0, Settings.MaxNameLength);
				}

				Settings._player_name = value;
			}
		}

		public static Colors PlayerColor = Colors.Red;
		public static Faces PlayerFace = Faces.Face_1;
		public static IPAddress RokuIP = null;
		public static string RokuName = "";

		public const string RokuSearchAddress = "239.255.255.250";
		public const int RokuSearchPort = 1900;
		private static byte[] _roku_search_bytes = null;
		public static byte[] RokuSearchBytes {
			get {
				if (_roku_search_bytes == null) {
					List<byte> bytes = new List<byte>();
					bytes.AddRange(Encoding.ASCII.GetBytes("M-SEARCH * HTTP/1.1"));
					bytes.Add(10);
					bytes.AddRange(Encoding.ASCII.GetBytes("Host: " + RokuSearchAddress + ":" + RokuSearchPort));
					bytes.Add(10);
					bytes.AddRange(Encoding.ASCII.GetBytes("Man: \"ssdp:discover\""));
					bytes.Add(10);
					bytes.AddRange(Encoding.ASCII.GetBytes("ST: roku:ecp"));
					bytes.Add(10);
					_roku_search_bytes = bytes.ToArray();
				}

				return _roku_search_bytes;
			}
		}

		public static readonly string[] MusicLinks = {
			"https://www.audiotool.com/track/instantly/",
			"https://www.audiotool.com/track/electric_love_zone_remix_preview/",
			"https://www.audiotool.com/track/lowlife_original_mix/",
			"https://www.audiotool.com/track/boogie_cat_s_bar/",
			"https://www.audiotool.com/track/the_seeker-l9htihhw/",
			"https://www.audiotool.com/track/conversion/",
			"https://www.audiotool.com/track/dead_space-svuvi/",
		};
	}
}
