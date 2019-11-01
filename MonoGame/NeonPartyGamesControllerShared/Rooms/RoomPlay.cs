using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;

namespace NeonPartyGamesController.Rooms
{
	public class RoomPlay : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			var trackpad = Engine.SpawnInstance<Trackpad>();
			var player = new Player(trackpad);
			Engine.SpawnInstance(player);
			if (Settings.RokuIP != null) {
				var player_networking_control = new PlayerNetworkingControl(player, Settings.RokuIP, Settings.RokuPort);
				Engine.SpawnInstance(player_networking_control);
			}

			int button_y = 75;
			int button_spread_x = 150;
			int center = Engine.Game.CanvasWidth / 2;
			var button_move_trackpad = new ButtonMoveTrackpad(center - button_spread_x / 2 - button_spread_x, button_y, trackpad);
			var button_increase_trackpad = new ButtonIncreaseTrackpad(center - button_spread_x / 2 , button_y, trackpad);
			var button_reduce_trackpad = new ButtonReduceTrackpad(center + button_spread_x / 2 , button_y, trackpad);
			var button_reset_trackpad = new ButtonResetTrackpad(center + button_spread_x / 2 + button_spread_x, button_y, trackpad);

			Engine.SpawnInstance(button_move_trackpad);
			Engine.SpawnInstance(button_increase_trackpad);
			Engine.SpawnInstance(button_reduce_trackpad);
			Engine.SpawnInstance(button_reset_trackpad);

			var button_back_avoid_trackpad = new ButtonBackAvoidTrackpad(trackpad);
			Engine.SpawnInstance(button_back_avoid_trackpad);
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
