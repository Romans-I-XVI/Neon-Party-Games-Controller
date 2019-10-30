using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController
{
	public static class Settings
	{
		public static void Initialize() {
			string name = SaveDataHandler.LoadData(SavePaths.PlayerName).Trim();
			if (!string.IsNullOrEmpty(name))
				_player_name = name;

			string color = SaveDataHandler.LoadData(SavePaths.PlayerColor).Trim();
			if (!string.IsNullOrEmpty(color)) {
				bool success = int.TryParse(color, out int color_int);
				if (success)
					_player_color = (Colors)color_int;
			}

			string face = SaveDataHandler.LoadData(SavePaths.PlayerFace).Trim();
			if (!string.IsNullOrEmpty(face)) {
				bool success = int.TryParse(face, out int face_int);
				if (success)
					_player_face = (Faces)face_int;
			}

			string roku_ip = SaveDataHandler.LoadData(SavePaths.RokuIP).Trim();
			if (!string.IsNullOrEmpty(roku_ip)) {
				bool success = IPAddress.TryParse(roku_ip, out IPAddress roku_ip_address);
				if (success)
					_roku_ip = roku_ip_address;
			}

			string roku_name = SaveDataHandler.LoadData(SavePaths.RokuName).Trim();
			if (!string.IsNullOrEmpty(roku_name))
				_roku_name = roku_name;

			if (_roku_ip != null) {
				Task.Run(async () => {
					bool alive = await RokuECP.PingRoku(roku_ip);
					if (!alive) {
						RokuIP = null;
						RokuName = "";
					}
				});
			}
		}

		public static class SavePaths
		{
			public const string PlayerName = "PlayerName.txt";
			public const string PlayerColor = "PlayerColor.txt";
			public const string PlayerFace = "PlayerFace.txt";
			public const string RokuIP = "RokuIP.txt";
			public const string RokuName = "RokuName.txt";
		}
		public const int MaxNameLength = 25;

		private static string _player_name = "Mr. Null";
		private static Colors _player_color = Colors.Red;
		private static Faces _player_face = Faces.Face_1;
		private static IPAddress _roku_ip = null;
		private static string _roku_name = "";

		public static string PlayerName {
			get => Settings._player_name;
			set {
				value = value.Trim();
				if (value.Length > Settings.MaxNameLength) {
					value = value.Substring(0, Settings.MaxNameLength);
				}
				SaveDataHandler.SaveData(value, SavePaths.PlayerName);
				_player_name = value;
			}
		}

		public static Colors PlayerColor {
			get => _player_color;
			set {
				SaveDataHandler.SaveData(((int)value).ToString(), SavePaths.PlayerColor);
				_player_color = value;
			}
		}

		public static Faces PlayerFace {
			get => _player_face;
			set {
				SaveDataHandler.SaveData(((int)value).ToString(), SavePaths.PlayerFace);
				_player_face = value;
			}
		}

		public static IPAddress RokuIP {
			get => _roku_ip;
			set {
				string write_value = (value != null) ? value.ToString() : "";
				SaveDataHandler.SaveData(write_value, SavePaths.RokuIP);
				_roku_ip = value;
			}
		}

		public static string RokuName {
			get => _roku_name;
			set {
				SaveDataHandler.SaveData(value, SavePaths.RokuName);
				_roku_name = value;
			}
		}

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
