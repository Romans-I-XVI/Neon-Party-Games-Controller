using System.Collections.Generic;

namespace NeonPartyGamesController
{
	public static class HapticFeedbackPlayer
	{
		public static Dictionary<HapticEffect, int[]> Haptics = new Dictionary<HapticEffect, int[]> {
			[HapticEffect.ShortBuzzWeak] = new []{20, 30, 20}
		};

		public static void Play(byte value) {
			if (HapticFeedbackPlayer.Haptics.ContainsKey((HapticEffect)value))
				VibrationHelper.Vibrate(HapticFeedbackPlayer.Haptics[(HapticEffect)value]);
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
