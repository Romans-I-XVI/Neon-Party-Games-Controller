using MonoEngine;
using NeonPartyGamesController.Rooms;

namespace NeonPartyGamesController
{
	public class NeonPartyGamesControllerGame : EngineGame
	{
#if ANDROID
        public static Microsoft.Devices.Sensors.Accelerometer Accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
        public static Android.OS.Vibrator Vibrator;
#endif

		public NeonPartyGamesControllerGame() : base(1280, 720, 0, 0) {
#if !NETFX_CORE
			this.Graphics.SynchronizeWithVerticalRetrace = false;
#endif
			this.IsFixedTimeStep = false;
			this.Content.RootDirectory = "Content";

			this.Graphics.IsFullScreen = false;
#if !ANDROID && !IOS && !PLAYSTATION4
			this.Graphics.HardwareModeSwitch = false;
			this.IsMouseVisible = true;
#endif

#if NETFX_CORE
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(1280, 720);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode = Windows.UI.ViewManagement.FullScreenSystemOverlayMode.Minimal;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs args) => { args.Handled = true; };
#endif
#if !PLAYSTATION4
			this.Window.AllowUserResizing = true;
#endif
			this.Window.AllowAltF4 = true;
		}

		protected override void Initialize() {
			base.Initialize();
#if ANDROID
            if (NeonPartyGamesControllerGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.Ready)
            {
                NeonPartyGamesControllerGame.Accelerometer.Start();
            }
#endif
		}

		protected override void LoadContent() {
			base.LoadContent();

			Engine.ChangeRoom<RoomMain>();
		}
	}
}
