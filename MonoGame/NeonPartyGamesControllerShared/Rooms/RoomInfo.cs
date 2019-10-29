using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomInfo : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<ButtonBack>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
