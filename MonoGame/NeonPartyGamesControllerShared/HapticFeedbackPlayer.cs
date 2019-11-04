namespace NeonPartyGamesController
{
	public static class HapticFeedbackPlayer
	{
		public static void Play(byte value) {
		}
	}

	public enum HapticEffect : byte
	{
		ImpactStrong = 96,
		ImpactWeak = 98,
		BuzzStrong = 30,
		BuzzWeak = 32
	}
}
