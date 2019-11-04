using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController
{
	public static class VibrationHelper
	{
		public static int StandardVibrationLength = 18;

		public static bool Vibrate()
		{
			if (Engine.Room is RoomSelectFace) {
				VibrationHelper.FetchAndPlayVibration();
				return true;
			}
			return Vibrate(StandardVibrationLength);
		}

		private static void FetchAndPlayVibration() {
			try {
				var task = Task.Run(() => RokuECP.Client.GetByteArrayAsync("http://192.168.1.245/vibrator.txt"));
				task.Wait();
				var bytes = task.Result;
				string res = Encoding.ASCII.GetString(bytes);
				System.Diagnostics.Debug.WriteLine(res);
				if (!string.IsNullOrEmpty(res)) {
					var string_array = res.Split(',');
					int[] test = new int[string_array.Length];
					for (int i = 0; i < test.Length; i++) {
						test[i] = int.Parse(string_array[i]);
					}
					VibrationHelper.Vibrate(test);
				}

			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e);
			}
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

		public static void Vibrate(int[] on_off_duration_array) {
			if (on_off_duration_array == null)
				return;
			var array = (int[])on_off_duration_array.Clone();

			Task.Run(() => {
				int index = 0;
				while (index < array.Length) {
					if (index % 2 == 0)
						VibrationHelper.Vibrate(array[index]);
					Thread.Sleep(array[index]);
					index++;
				}
			});
		}
	}
}
