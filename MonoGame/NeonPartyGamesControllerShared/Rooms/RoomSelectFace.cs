using System;
using System.Collections.Generic;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Entities.Buttons;
using NeonPartyGamesController.Enums;

namespace NeonPartyGamesController.Rooms
{
	public class RoomSelectFace : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<ButtonBack>();
			Engine.SpawnInstance<CurrentFaceDrawer>();

			this.SpawnFaces();
		}

		public override void onSwitchAway(Room next_room) {
		}

		private void SpawnFaces() {
			float layout_scale = 1f;
			float required_space = 1116f;
			if (Engine.Game.CanvasWidth < required_space)
				layout_scale = Engine.Game.CanvasWidth / required_space;
			float scale = 1.33f * layout_scale;
			int spread = (int)(186 * layout_scale);
			int start_x = Engine.Game.CanvasWidth / 2;
			int start_y = 440;

			for (int x = 0; x < 6; x++) {
				int offset_x = (spread * x + spread / 2) - (spread * 6) / 2;
				for (int y = 0; y < 2; y++) {
					int offset_y = y * spread;
					int face_int = (x + (y * 6)) + 1;
					Faces face = Enum.Parse<Faces>("Face_" + face_int);
					var button = new ButtonFace(face, start_x + offset_x, start_y + offset_y, scale);
					Engine.SpawnInstance(button);
				}
			}
		}
	}
}
