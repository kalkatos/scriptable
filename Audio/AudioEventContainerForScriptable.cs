using UnityEngine;
using Kalkatos.UnityGame.Audio;

namespace Kalkatos.UnityGame.Scriptable.Audio
{
	public class AudioEventContainerForScriptable : MonoBehaviour
	{
		[SerializeField] private SignalToMusic[] musicEvents;
		[SerializeField] private SignalToSfx[] sfxEvents;

		private void Awake ()
		{
			for (int i = 0; i < musicEvents.Length; i++)
				musicEvents[i].RegisterSignals();
			for (int i = 0; i < sfxEvents.Length; i++)
				sfxEvents[i].RegisterSignals();
		}

		private void OnDestroy ()
		{
			for (int i = 0; i < musicEvents.Length; i++)
				musicEvents[i].UnregisterSignals();
			for (int i = 0; i < sfxEvents.Length; i++)
				sfxEvents[i].UnregisterSignals();
		}
	}

	[System.Serializable]
	public class SignalToMusic
	{
		public Signal[] Signals;
		public BackgroundMusic Music;

		public void Play ()
		{
			AudioController.PlayMusic(Music);
		}

		public void Play (string s) => Play();
		public void Play (float f) => Play();
		public void Play (int i) => Play();
		public void Play (bool b) => Play();

		public void RegisterSignals ()
		{
			for (int i = 0; i < Signals.Length; i++)
			{
				Signal signal = Signals[i];
				if (signal is TypedSignal<string>)
				{
					var typed = (TypedSignal<string>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<float>)
				{
					var typed = (TypedSignal<float>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<int>)
				{
					var typed = (TypedSignal<int>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<bool>)
				{
					var typed = (TypedSignal<bool>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				signal.OnSignalEmitted.AddListener(Play); 
			}
		}

		public void UnregisterSignals ()
		{
			for (int i = 0; i < Signals.Length; i++)
			{
				Signal signal = Signals[i];
				if (signal is TypedSignal<string>)
				{
					var typed = (TypedSignal<string>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<float>)
				{
					var typed = (TypedSignal<float>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<int>)
				{
					var typed = (TypedSignal<int>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<bool>)
				{
					var typed = (TypedSignal<bool>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				signal.OnSignalEmitted.RemoveListener(Play);
			}
		}
	}

	[System.Serializable]
	public class SignalToSfx
	{
		public Signal[] Signals;
		public SoundEffect SoundEffect;

		public void Play ()
		{
			AudioController.PlaySfx(SoundEffect);
		}

		public void Play (string s) => Play();
		public void Play (float f) => Play();
		public void Play (int i) => Play();
		public void Play (bool b) => Play();

		public void RegisterSignals ()
		{
			for (int i = 0; i < Signals.Length; i++)
			{
				Signal signal = Signals[i];
				if (signal is TypedSignal<string>)
				{
					var typed = (TypedSignal<string>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<float>)
				{
					var typed = (TypedSignal<float>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<int>)
				{
					var typed = (TypedSignal<int>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				if (signal is TypedSignal<bool>)
				{
					var typed = (TypedSignal<bool>)signal;
					typed.OnSignalEmittedWithParam.AddListener(Play);
					return;
				}
				signal.OnSignalEmitted.AddListener(Play);
			}
		}

		public void UnregisterSignals ()
		{
			for (int i = 0; i < Signals.Length; i++)
			{
				Signal signal = Signals[i];
				if (signal is TypedSignal<string>)
				{
					var typed = (TypedSignal<string>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<float>)
				{
					var typed = (TypedSignal<float>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<int>)
				{
					var typed = (TypedSignal<int>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				if (signal is TypedSignal<bool>)
				{
					var typed = (TypedSignal<bool>)signal;
					typed.OnSignalEmittedWithParam.RemoveListener(Play);
					return;
				}
				signal.OnSignalEmitted.RemoveListener(Play);
			}
		}
	}
}