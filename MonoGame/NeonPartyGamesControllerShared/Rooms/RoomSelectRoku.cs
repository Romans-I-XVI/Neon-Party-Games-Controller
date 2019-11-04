using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomSelectRoku : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<ButtonBack>();
			Engine.SpawnInstance<CurrentRokuDrawer>();

			float layout_scale = 1f;
			var button_manually_enter_ip= new ButtonManuallyEnterIP(Engine.Game.CanvasWidth / 2, 620, layout_scale);
			Engine.SpawnInstance(button_manually_enter_ip);
			Engine.SpawnInstance<RokuIPButtonSpawner>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
