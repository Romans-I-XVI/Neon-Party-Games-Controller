namespace NeonPartyGamesController
{
	public static class VibrationHelper
	{
		public static int StandardVibrationLength = 18;

		public static bool Vibrate()
		{
			return Vibrate(StandardVibrationLength);
		}

		public static bool Vibrate(int milliseconds)
		{
#if ANDROID || IOS || NETFX_CORE
			try {
				Xamarin.Essentials.Vibration.Vibrate(milliseconds);
				return true;
			} catch {
				return false;
			}
#else
            return false;
#endif
		}
	}
}
