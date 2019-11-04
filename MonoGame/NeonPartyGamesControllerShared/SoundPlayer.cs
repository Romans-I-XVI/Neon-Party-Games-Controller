using Microsoft.Xna.Framework.Audio;

namespace NeonPartyGamesController
{
	public static class SoundPlayer
	{
		private static readonly AvailableSounds[][] SoundsList = {
			new [] {
				AvailableSounds.hit_custom,
				AvailableSounds.win,
				AvailableSounds.lose,
			},
			new [] {
				AvailableSounds.geometry_poof,
				AvailableSounds.geometry_negative_poof
			},
			new [] {
				AvailableSounds.tag_it,
				AvailableSounds.tag_not_it
			},
			new [] {
				AvailableSounds.puck_hit,
				AvailableSounds.puck_team_score,
				AvailableSounds.puck_other_team_score
			},
			new [] {
				AvailableSounds.squared_death
			},
			new [] {
				AvailableSounds.sumo_out_of_arena,
				AvailableSounds.sumo_out_of_arena
			},
			new [] {
				AvailableSounds.cell_absorb_mini,
				AvailableSounds.cell_booster_grow,
				AvailableSounds.cell_booster_shrink,
				AvailableSounds.cell_booster_shrink_attack,
				AvailableSounds.cell_booster_magnet_up,
				AvailableSounds.cell_booster_magnet_down,
				AvailableSounds.cell_booster_speed_up,
				AvailableSounds.cell_booster_speed_down,
				AvailableSounds.cell_death
			}
		};

		public static void Play(byte category, byte identifier, byte value) {
			SoundEffect sound = null;
			float? volume = null;
			if (category == 0 && identifier == 0) {
				volume = (value - 20) / 100f;
				if (volume < 0)
					volume = 0;
				else if (volume > 1)
					volume = 1;
			}

			if (SoundPlayer.SoundsList.Length >= category) {
				if (SoundPlayer.SoundsList[category].Length >= identifier) {
					sound = ContentHolder.Get(SoundPlayer.SoundsList[category][identifier]);
				}
			}

			if (sound != null) {
				if (volume != null) {
					sound.TryPlay((float)volume, 0, 0);
				} else {
					sound.TryPlay();
				}
			}
		}
	}
}
