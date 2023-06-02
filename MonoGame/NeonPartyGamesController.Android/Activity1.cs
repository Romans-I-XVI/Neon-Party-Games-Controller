using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace NeonPartyGamesController
{
	[Activity(Label = "@string/ApplicationName"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, RoundIcon = "@drawable/iconround"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.UserLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	public class Activity1 : AndroidGameActivity, ViewTreeObserver.IOnPreDrawListener
	{
		private static Game Game = null;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			NeonPartyGamesControllerGame.AndroidContext = this;

			View game_view;
			if (Game == null) {
				Xamarin.Essentials.Platform.Init(this, bundle);

				Game = new NeonPartyGamesControllerGame();
				game_view = (View)Game.Services.GetService(typeof(View));
				((NeonPartyGamesControllerGame)Game).ExitEvent += () => MoveTaskToBack(true);

				Game.Run();
			} else {
				game_view = (View)Game.Services.GetService(typeof(View));
				ViewGroup parent = (ViewGroup)game_view.Parent;
				if (parent != null) {
					parent.RemoveView(game_view);
				}
			}

			SetContentView(game_view);
			FindViewById(Android.Resource.Id.Content)?.ViewTreeObserver?.AddOnPreDrawListener(this);
		}

		public bool OnPreDraw() {
			if (Engine.Room != null) {
				FindViewById(Android.Resource.Id.Content)?.ViewTreeObserver?.RemoveOnPreDrawListener(this);
				return true;
			}

			return false;
		}

		public override void OnWindowFocusChanged(bool has_focus) {
			if (has_focus)
				this.MakeFullScreen();
			NeonPartyGamesControllerGame.HasFocus = has_focus;
			base.OnWindowFocusChanged(has_focus);
		}

		protected void MakeFullScreen() {
			if (Build.VERSION.SdkInt >= BuildVersionCodes.R) {
				Window.SetDecorFitsSystemWindows(false);
				Window.InsetsController.Hide(WindowInsets.Type.SystemBars());
				Window.InsetsController.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
            } else {
#pragma warning disable 618
				var uiOptions =
					SystemUiFlags.HideNavigation |
					SystemUiFlags.LayoutFullscreen |
					SystemUiFlags.LayoutHideNavigation |
					SystemUiFlags.LayoutStable |
					SystemUiFlags.Fullscreen |
					SystemUiFlags.ImmersiveSticky;

				Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
#pragma warning restore 618
			}
		}

		public override void OnRequestPermissionsResult(int request_code, string[] permissions, [GeneratedEnum] Permission[] grant_results)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(request_code, permissions, grant_results);

			base.OnRequestPermissionsResult(request_code, permissions, grant_results);
		}
	}
}
