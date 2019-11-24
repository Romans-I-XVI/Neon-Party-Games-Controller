using System;
using Foundation;
using UIKit;

namespace NeonPartyGamesController
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static NeonPartyGamesControllerGame game;

        internal static void RunGame()
        {
            game = new NeonPartyGamesControllerGame();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
