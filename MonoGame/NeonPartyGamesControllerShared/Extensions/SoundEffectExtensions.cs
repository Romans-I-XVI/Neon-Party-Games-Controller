using Microsoft.Xna.Framework.Audio;
using MonoEngine;

namespace NeonPartyGamesController
{
	public static class SoundEffectExtensions
	{
		public static bool TryPlay(this SoundEffect sound_effect, float volume, float pitch, float pan) {
			bool played = false;
			Utilities.Try(() => played = sound_effect.Play(volume, pitch, pan));
			return played;
		}

		public static bool TryPlay(this SoundEffect sound_effect) {
			bool played = false;
			Utilities.Try(() => played = sound_effect.Play());
			return played;
		}
	}
}
