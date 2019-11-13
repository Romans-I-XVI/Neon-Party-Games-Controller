using System;
using MonoEngine;

namespace NeonPartyGamesController
{
	public static class PlatformFunctions
	{
		public static bool IsDialogOpen { get; private set; }

		public static async void OpenInputDialog(string title, Action<string> callback, int max_length = 0, object[] args = null) {
			if (PlatformFunctions.IsDialogOpen)
				return;

			PlatformFunctions.IsDialogOpen = true;
			string result = "";

			Engine.Pause();
#if ANDROID
			var builder = new Android.App.AlertDialog.Builder(NeonPartyGamesControllerGame.AndroidContext);
			var input = new Android.Widget.EditText(NeonPartyGamesControllerGame.AndroidContext);
			var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();

			builder.SetTitle(title);
			if (args != null && args.Length > 0)
				input.InputType = (Android.Text.InputTypes)args[0];
			else
				input.InputType = Android.Text.InputTypes.ClassText;
			if (max_length > 0)
				input.SetFilters(new Android.Text.IInputFilter[]{ new Android.Text.InputFilterLengthFilter(max_length) });
			builder.SetView(input);
			builder.SetPositiveButton("OK", (sender_alert, sender_args) => {
				VibrationHelper.Vibrate();
				result = input.Text;
			});
			builder.SetOnDismissListener(new OnDismissListener(() => tcs.TrySetResult(true)));
			builder.Show();
			await tcs.Task;
#elif IOS
#elif NETFX_CORE
            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var input_text_box = new Windows.UI.Xaml.Controls.TextBox
                {
                    AcceptsReturn = false,
                    Height = 32
                };
				if (max_length > 0)
					input_text_box.MaxLength = max_length;
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
			await System.Threading.Tasks.Task.Run(() => {
				var debugger = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
				debugger?.OpenConsoleWithCustomEvaluator(value => result = value);
				while (debugger != null && debugger.ConsoleOpen) {
					System.Threading.Thread.Sleep(1);
				}
			});
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
			var builder = new Android.App.AlertDialog.Builder(NeonPartyGamesControllerGame.AndroidContext);
			var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();

			builder.SetTitle(title);
			builder.SetMessage(message);
			builder.SetPositiveButton("OK", (sender_alert, sender_args) => {
				VibrationHelper.Vibrate();
			});
			builder.SetOnDismissListener(new OnDismissListener(() => tcs.TrySetResult(true)));
			builder.Show();
			await tcs.Task;
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

#if ANDROID
		private sealed class OnDismissListener : Java.Lang.Object, Android.Content.IDialogInterfaceOnDismissListener
		{
			private readonly Action action;

			public OnDismissListener(Action action)
			{
				this.action = action;
			}

			public void OnDismiss(Android.Content.IDialogInterface dialog)
			{
				this.action();
			}
		}
#endif
	}
}
