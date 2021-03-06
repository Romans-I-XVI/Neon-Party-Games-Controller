using System;
using System.Diagnostics;
using MonoEngine;

namespace NeonPartyGamesController.Entities.Buttons
{
	public class ButtonHyperlink : Button
	{
		private static Sprite _sprite => new Sprite(SpriteSheetHolder.SpriteSheet.GetRegion(AvailableRegions.Buttons.Hyperlink));
		private static Circle _collider_circle => new Circle(0, 0, 25);

		public ButtonHyperlink(int x, int y, string link) : base(x, y, 1, _sprite, _collider_circle, ButtonHyperlink.GetOnClickAction(link)) {
			this.ShouldPlaySoundOnClick = false;
		}

		private static Action GetOnClickAction(string link) {
			return () => {
#if ANDROID || IOS || NETFX_CORE
				try {
					Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => {
						Xamarin.Essentials.Browser.OpenAsync(link);
					});
				} catch {}
#else
				try {
					var psi = new ProcessStartInfo
					{
						FileName = link,
						UseShellExecute = true
					};
					Process.Start(psi);
				} catch (Exception e) {
					Debug.WriteLine(e);
				}
#endif
			};
		}

	}
}
