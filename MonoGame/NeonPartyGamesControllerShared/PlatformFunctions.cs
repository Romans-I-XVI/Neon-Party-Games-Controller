using System;
using MonoEngine;

namespace NeonPartyGamesController
{
	public static class PlatformFunctions
	{
		public static bool IsDialogOpen { get; private set; }

		public static async void OpenInputDialog(string title, Action<string> callback, object[] args = null) {
			if (PlatformFunctions.IsDialogOpen)
				return;

			PlatformFunctions.IsDialogOpen = true;
			string result = "";

			Engine.Pause();
#if ANDROID
			Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(NeonPartyGamesControllerGame.AndroidContext);
			Android.Widget.EditText input = new Android.Widget.EditText(NeonPartyGamesControllerGame.AndroidContext);

			builder.SetTitle(title);
			if (args != null && args.Length > 0)
				input.InputType = (Android.Text.InputTypes)args[0];
			else
				input.InputType = Android.Text.InputTypes.ClassText;
			input.SetFilters(new Android.Text.IInputFilter[]{ new Android.Text.InputFilterLengthFilter(Settings.MaxNameLength) });
			builder.SetView(input);
			builder.SetPositiveButton("OK", (sender_alert, sender_args) => {
				VibrationHelper.Vibrate();
				if (!string.IsNullOrWhiteSpace(input.Text))
				{
					Settings.PlayerName = input.Text.Trim();
				}
			});
			builder.Show();
#elif IOS
#elif NETFX_CORE
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var input_text_box = new Windows.UI.Xaml.Controls.TextBox
                {
                    AcceptsReturn = false,
                    Height = 32
                };
                var dialog = new Windows.UI.Xaml.Controls.ContentDialog
                {
                    Content = input_text_box,
                    Title = title,
                    PrimaryButtonText = "Ok",
                    IsSecondaryButtonEnabled = true,
                    SecondaryButtonText = "Cancel",
                    DefaultButton = Windows.UI.Xaml.Controls.ContentDialogButton.Primary
                };
                if (await dialog.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                    result = input_text_box.Text;
                else
                    result = "";
            });
#else
	#if DEBUG
			var debugger = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
			debugger?.OpenConsoleWithCustomEvaluator(callback);
	#endif
#endif
			Engine.Resume();
			PlatformFunctions.IsDialogOpen = false;

			if (!string.IsNullOrWhiteSpace(result)) {
				callback?.Invoke(result);
			}
		}

		public static async void OpenMessageDialog(string title, string message) {
			if (PlatformFunctions.IsDialogOpen)
				return;

			PlatformFunctions.IsDialogOpen = true;
			Engine.Pause();
#if ANDROID
			Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(NeonPartyGamesControllerGame.AndroidContext);
			builder.SetTitle(title);
			builder.SetMessage(message);
			builder.SetPositiveButton("OK", (sender_alert, sender_args) => {
				VibrationHelper.Vibrate();
			});
			builder.Show();
#elif IOS
#elif NETFX_CORE
			await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () =>
			{
				var dialog = new Windows.UI.Popups.MessageDialog(message, title);
				await dialog.ShowAsync();
			});
#endif
			Engine.Resume();
			PlatformFunctions.IsDialogOpen = false;
		}
	}
}
