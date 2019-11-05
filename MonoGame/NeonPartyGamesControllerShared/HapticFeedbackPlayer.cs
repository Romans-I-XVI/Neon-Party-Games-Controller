using System.Collections.Generic;

namespace NeonPartyGamesController
{
	public static class HapticFeedbackPlayer
	{
		public static Dictionary<HapticEffect, int[]> Haptics = new Dictionary<HapticEffect, int[]> {
			[HapticEffect.ImpactStrong] = new[] {32},
			[HapticEffect.ImpactWeak] = new[] {25},
			[HapticEffect.ShortBuzzStrong] = new[] {40, 15, 40},
			[HapticEffect.ShortBuzzWeak] = new[] {20, 30, 20},
			[HapticEffect.LongBuzzStrong] = new[] {50, 0, 50, 0, 50, 0, 50, 0, 50, 0, 50},
			[HapticEffect.TripleStrongClick] = new[] {25, 50, 25, 50, 25}
		};

		public static void Play(byte value) {
			if (HapticFeedbackPlayer.Haptics.ContainsKey((HapticEffect)value)) {
				var on_off_duration_array = HapticFeedbackPlayer.Haptics[(HapticEffect)value];
				if (on_off_duration_array.Length == 1)
					VibrationHelper.Vibrate(on_off_duration_array[0]);
				else
					VibrationHelper.Vibrate(on_off_duration_array);
			}
		}
	}

	public enum HapticEffect : byte
	{
		ImpactStrong = 96,
		ImpactWeak = 98,
		ShortBuzzStrong = 30,
		ShortBuzzWeak = 32,
		LongBuzzStrong = 27,
		TripleStrongClick = 21
	}
}
