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

			float required_space = 1116f;
			float layout_scale = (Engine.Game.CanvasWidth < required_space) ? Engine.Game.CanvasWidth / required_space : 1f;
			this.SpawnFaceButtons(layout_scale);
			this.SpawnColorButtons(layout_scale);
			this.SpawnSetNameButton(layout_scale);
		}

		public override void onSwitchAway(Room next_room) {
		}

		private void SpawnFaceButtons(float layout_scale) {
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

		private void SpawnColorButtons(float layout_scale) {
			float scale = 1f * layout_scale;
			int spread = (int)(128 * layout_scale);
			int start_x = (int)(Engine.Game.CanvasWidth / 2 + 240 * scale);
			int start_y = 128 + spread / 2;

			for (int x = 0; x < 3; x++) {
				int offset_x = x * spread;
				for (int y = 0; y < 2; y++) {
					int offset_y = spread * y - spread / 2;
					int color_int = (x + (y * 3));
					var button = new ButtonColor((Colors)color_int, start_x + offset_x, start_y + offset_y, scale);
					Engine.SpawnInstance(button);
				}
			}
		}

		private void SpawnSetNameButton(float layout_scale) {

		}
	}
}
