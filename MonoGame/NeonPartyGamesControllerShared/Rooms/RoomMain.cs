using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomMain : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			int button_spread = 420;

			var button_select_roku = new ButtonSelectRoku(Engine.Game.CanvasWidth / 2 - button_spread, Engine.Game.CanvasHeight - 200);
			var button_select_face = new ButtonSelectFace(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight - 200);
			var button_play = new ButtonPlay(Engine.Game.CanvasWidth / 2 + button_spread, Engine.Game.CanvasHeight - 200);
			var button_info = new ButtonInfo(Engine.Game.CanvasWidth, 0);

			Engine.SpawnInstance(button_select_roku);
			Engine.SpawnInstance(button_select_face);
			Engine.SpawnInstance(button_play);
			Engine.SpawnInstance(button_info);
			Engine.SpawnInstance<CurrentFaceDrawer>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
