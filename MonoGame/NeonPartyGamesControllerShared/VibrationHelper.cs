namespace NeonPartyGamesController
{
	public static class VibrationHelper
	{
		public static int StandardVibrationLength = 18;

#if ANDROID
		private static Android.OS.Vibrator Vibrator;
		public static void Init(Android.OS.Vibrator vibrator) {
			VibrationHelper.Vibrator = vibrator;
		}
#endif

		public static bool Vibrate()
		{
			return Vibrate(StandardVibrationLength);
		}

		public static bool Vibrate(int milliseconds)
		{
#if ANDROID
			VibrationHelper.Vibrator.Vibrate(milliseconds);
			return true;
#else
            return false;
#endif
		}
	}
}
