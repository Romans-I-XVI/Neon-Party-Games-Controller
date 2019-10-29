using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomMain : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			float layout_scale = 1f;
			float required_space = 1280f;
			if (Engine.Game.CanvasWidth < required_space)
				layout_scale = Engine.Game.CanvasWidth / required_space;

			int button_spread = (int)(420 * layout_scale);

			var button_select_roku = new ButtonSelectRoku(Engine.Game.CanvasWidth / 2 - button_spread, Engine.Game.CanvasHeight - 200, layout_scale);
			var button_select_face = new ButtonSelectFace(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight - 200, layout_scale);
			var button_play = new ButtonPlay(Engine.Game.CanvasWidth / 2 + button_spread, Engine.Game.CanvasHeight - 200, layout_scale);
			var button_info = new ButtonInfo(Engine.Game.CanvasWidth, 0);

			Engine.SpawnInstance(button_select_roku);
			Engine.SpawnInstance(button_select_face);
			Engine.SpawnInstance(button_play);
			Engine.SpawnInstance(button_info);
			Engine.SpawnInstance<CurrentRokuDrawer>();
			Engine.SpawnInstance<CurrentFaceDrawer>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
