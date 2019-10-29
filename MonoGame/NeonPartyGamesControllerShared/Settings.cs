using System.Net;
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
	}
}
