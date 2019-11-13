using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using NeonPartyGamesController.Entities;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController
{
	public class NeonPartyGamesControllerGame : EngineGame
	{
		public const int ScreenHeight = 720;
		private static bool _hasFocus = true;
		public delegate void dgFocusChangeEvent(bool has_focus);
		public static event dgFocusChangeEvent FocusChangeEvent;

#if ANDROID
		public static Android.Content.Context AndroidContext;
#endif
		public static bool ExitGame = false;
		public delegate void dgExitEvent();
		public event dgExitEvent ExitEvent;

		public NeonPartyGamesControllerGame() : base(1280, NeonPartyGamesControllerGame.ScreenHeight, 0, 0) {
			this.CanvasWidth = NeonPartyGamesControllerGame.GetScreenWidth();
			this.BackgroundColor = Settings.BackgroundColor;
			this.Graphics.PreferredBackBufferWidth = this.CanvasWidth;
			this.Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

#if NETFX_CORE
            this.Graphics.SynchronizeWithVerticalRetrace = true;
#else
			this.Graphics.SynchronizeWithVerticalRetrace = false;
#endif
			this.IsFixedTimeStep = false;
			this.Content.RootDirectory = "Content";

#if ANDROID || IOS
			this.Graphics.IsFullScreen = true;
#else
	#if DEBUG
			this.Graphics.IsFullScreen = false;
	#else
			this.Graphics.IsFullScreen = true;
	#endif
#endif
#if !ANDROID && !IOS && !PLAYSTATION4
			this.Graphics.HardwareModeSwitch = false;
			this.IsMouseVisible = true;
#endif

#if NETFX_CORE
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(this.CanvasWidth, NeonPartyGamesControllerGame.ScreenHeight);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode = Windows.UI.ViewManagement.FullScreenSystemOverlayMode.Minimal;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs args) => { args.Handled = true; };
#endif
#if !PLAYSTATION4
			this.Window.AllowUserResizing = true;
#endif
			this.Window.AllowAltF4 = true;
		}

		public static bool HasFocus {
			get => NeonPartyGamesControllerGame._hasFocus;
			set {
				NeonPartyGamesControllerGame._hasFocus = value;
				NeonPartyGamesControllerGame.FocusChangeEvent?.Invoke(NeonPartyGamesControllerGame._hasFocus);
			}
		}

		private static int GetScreenWidth()
		{
			float screen_pixel_width;
			float screen_pixel_height;

#if ANDROID
            Android.Util.DisplayMetrics displayMetrics = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);
            screen_pixel_width = (float)displayMetrics.WidthPixels;
            screen_pixel_height = (float)displayMetrics.HeightPixels;
#else
			screen_pixel_width = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			screen_pixel_height = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#endif

			float resolution_percent = (float)NeonPartyGamesControllerGame.ScreenHeight / screen_pixel_height;
			return (int)(screen_pixel_width * resolution_percent);
		}

		private static int GetScreenWidth(int fake_display_width, int fake_display_height)
		{
			float resolution_percent = (float)NeonPartyGamesControllerGame.ScreenHeight / (float)fake_display_height;
			int screen_width = (int)((float)fake_display_width * resolution_percent);

			Debug.WriteLine("----- Fake Screen Size Enabled ------");
			Debug.WriteLine("Width: " + screen_width);
			Debug.WriteLine("Height: " + NeonPartyGamesControllerGame.ScreenHeight);
			Debug.WriteLine("----- Fake Screen Size Enabled ------");
			return screen_width;
		}

		protected override void LoadContent() {
			base.LoadContent();
			ContentHolder.Init(this);
			SpriteSheetHolder.Init(
				AvailableTextures.spritesheet
			);
			Settings.Initialize();
#if DEBUG
			Engine.SpawnInstance<NeonPartyGamesControllerDebugger>();
#endif
#if !ANDROID && !IOS
			Engine.SpawnInstance<FullscreenControl>();
#endif
			Engine.ChangeRoom<RoomMain>();
		}

		protected override void Update(GameTime game_time) {
			base.Update(game_time);
			if (NeonPartyGamesControllerGame.ExitGame) {
				NeonPartyGamesControllerGame.ExitGame = false;
				this.ExitEvent?.Invoke();
#if !ANDROID && !IOS
                this.Exit();
#endif
			}
		}
	}
}
