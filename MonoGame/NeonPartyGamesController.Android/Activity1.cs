using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace NeonPartyGamesController
{
	[Activity(Label = "@string/ApplicationName"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.UserLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	public class Activity1 : AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			this.MakeFullScreen();
			NeonPartyGamesControllerGame.Vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Android.Content.Context.VibratorService);
			var g = new NeonPartyGamesControllerGame();
			g.exitEvent += () => MoveTaskToBack(true);
			this.SetContentView((View)g.Services.GetService(typeof(View)));
			g.Run();
		}

		public override void OnWindowFocusChanged(bool has_focus) {
			if (has_focus)
				this.MakeFullScreen();
			base.OnWindowFocusChanged(has_focus);
		}

		protected void MakeFullScreen() {
			var ui_options =
				SystemUiFlags.HideNavigation; // |
			//SystemUiFlags.LayoutFullscreen |
			//SystemUiFlags.LayoutHideNavigation |
			//SystemUiFlags.LayoutStable |
			//SystemUiFlags.Fullscreen |
			//SystemUiFlags.ImmersiveSticky;

			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)ui_options;
		}
	}
}
