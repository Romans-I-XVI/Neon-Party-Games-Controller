using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using MonoEngine;
using NeonPartyGamesController.Rooms;
using System.Diagnostics;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonSetName : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.SetName));
		private static Rectangle _collider_rect => new Rectangle(-270 / 2, -140 / 2, 270, 140);

		public ButtonSetName(int x, int y, float scale) : base(x, y, scale, _sprite, _collider_rect, ButtonSetName.SetName) {

		}

		private static void SetName() {
            string title = "Enter Name";

#if ANDROID
			Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(NeonPartyGamesControllerGame.AndroidContext);
			Android.Widget.EditText input = new Android.Widget.EditText(NeonPartyGamesControllerGame.AndroidContext);

			builder.SetTitle(title);
			input.InputType = Android.Text.InputTypes.ClassText;
			input.SetFilters(new Android.Text.IInputFilter[]{ new Android.Text.InputFilterLengthFilter(Settings.MaxNameLength) });
			builder.SetView(input);
			builder.SetPositiveButton("OK", (sender_alert, args) => {
				VibrationHelper.Vibrate();
				if (!string.IsNullOrWhiteSpace(input.Text))
				{
					Settings.PlayerName = input.Text.Trim();
				}
			});
			builder.Show();
#elif IOS
#elif NETFX_CORE
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
            {
                TextBox inputTextBox = new TextBox
                {
                    AcceptsReturn = false,
                    Height = 32
                };
                ContentDialog dialog = new ContentDialog
                {
                    Content = inputTextBox,
                    Title = title,
                    PrimaryButtonText = "Ok",
                    IsSecondaryButtonEnabled = true,
                    SecondaryButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary
                };
                string result = "";
                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                    result = inputTextBox.Text;
                else
                    result = "";

                if (!string.IsNullOrWhiteSpace(result))
                {
                    Settings.PlayerName = result.Trim();
                }
            });
#else
	#if DEBUG
			var debugger = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
			if (debugger != null) {
				Action<string> evaluator = s => {
					if (s != null && s.Trim() != "") {
						Settings.PlayerName = s.Trim();
					};
				};
				debugger.OpenConsoleWithCustomEvaluator(evaluator);
			}
	#endif
#endif
		}
	}
}
