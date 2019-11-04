using System;

namespace NeonPartyGamesController
{
	public static class Program
	{
		[STAThread]
		static void Main() {
			using (var game = new NeonPartyGamesControllerGame())
				game.Run();
		}
	}
}
