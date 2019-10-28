using System.Collections.Generic;
using MonoEngine;

namespace NeonPartyGamesController.Rooms
{
	public class RoomMain : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<NetworkingTest>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
