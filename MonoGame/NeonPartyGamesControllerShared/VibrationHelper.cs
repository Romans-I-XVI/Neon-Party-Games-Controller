using System.Threading;
using System.Threading.Tasks;

namespace NeonPartyGamesController
{
	public static class VibrationHelper
	{
		private static uint _current_vibration_task_id = 0;
		public static int StandardVibrationLength = 20;

		public static bool Vibrate()
		{
			return Vibrate(StandardVibrationLength);
		}

		public static bool Vibrate(int milliseconds)
		{
#if ANDROID || NETFX_CORE
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

		public static void Vibrate(int[] on_off_duration_array) {
			if (on_off_duration_array == null)
				return;
			_current_vibration_task_id++;
			uint my_vibration_task_id = _current_vibration_task_id;
			var array = (int[])on_off_duration_array.Clone();

			Task.Run(() => {
				int index = 0;
				while (index < array.Length && _current_vibration_task_id == my_vibration_task_id) {
					if (index % 2 == 0)
						VibrationHelper.Vibrate(array[index]);
					Thread.Sleep(array[index]);
					index++;
				}
			});
		}
	}
}
