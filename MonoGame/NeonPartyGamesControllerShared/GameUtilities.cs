using System;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController
{
	public class GameUtilities
	{
		public static Vector2 MoveTowards(Vector2 current_pos, Vector2 dest_pos, float total_speed, bool do_not_overshoot = true) {
			float distance_x = current_pos.X - dest_pos.X;
			float distance_y = current_pos.Y - dest_pos.Y;
			float angle = (float)Math.Atan2(distance_y, distance_x);
			var speed_vector = VectorMath.HypotenuseToVector(total_speed, angle);
			float new_x = current_pos.X - speed_vector.X;
			float new_y = current_pos.Y - speed_vector.Y;

			if (do_not_overshoot) {
				if (Math.Abs(distance_x) < Math.Abs(speed_vector.X))
					new_x = dest_pos.X;

				if (Math.Abs(distance_y) < Math.Abs(speed_vector.Y))
					new_y = dest_pos.Y;
			}

			return new Vector2(new_x, new_y);
		}
	}
}
