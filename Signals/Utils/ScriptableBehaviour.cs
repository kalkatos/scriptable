using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	public class ScriptableBehaviour : MonoBehaviour
	{
		protected Dictionary<SignalBase, Action> listeners = new();
		protected Dictionary<SignalObject, Action<object>> objectListeners = new();
		protected Dictionary<SignalBool, Action<bool>> boolListeners = new();
		protected Dictionary<SignalInt, Action<int>> intListeners = new();
		protected Dictionary<SignalFloat, Action<float>> floatListeners = new();
		protected Dictionary<SignalString, Action<string>> stringListeners = new();

		protected virtual void Awake ()
		{
			Initialize();
			RegisterListeners();
		}

		protected virtual void Initialize () { }

		protected virtual void RegisterListeners () { }

		protected virtual void OnDestroy ()
		{
			foreach (var listener in listeners)
				listener.Key.OnEmitted -= listener.Value;
			listeners.Clear();
			foreach (var listener in objectListeners)
				listener.Key.OnEmittedWithParam -= listener.Value;
			objectListeners.Clear();
			foreach (var listener in boolListeners)
				listener.Key.OnEmittedWithParam -= listener.Value;
			boolListeners.Clear();
			foreach (var listener in intListeners)
				listener.Key.OnEmittedWithParam -= listener.Value;
			intListeners.Clear();
			foreach (var listener in floatListeners)
				listener.Key.OnEmittedWithParam -= listener.Value;
			floatListeners.Clear();
			foreach (var listener in stringListeners)
				listener.Key.OnEmittedWithParam -= listener.Value;
			stringListeners.Clear();
		}

		protected virtual void Listen (SignalBase signal, Action action)
		{
			listeners[signal] = action;
			signal.OnEmitted += action;
		}

		protected virtual void Listen (SignalObject signal, Action<object> action)
		{
			objectListeners[signal] = action;
			signal.OnEmittedWithParam += action;
		}

		protected virtual void Listen (SignalBool signal, Action<bool> action)
		{
			boolListeners[signal] = action;
			signal.OnEmittedWithParam += action;
		}

		protected virtual void Listen (SignalInt signal, Action<int> action)
		{
			intListeners[signal] = action;
			signal.OnEmittedWithParam += action;
		}

		protected virtual void Listen (SignalFloat signal, Action<float> action)
		{
			floatListeners[signal] = action;
			signal.OnEmittedWithParam += action;
		}

		protected virtual void Listen (SignalString signal, Action<string> action)
		{
			stringListeners[signal] = action;
			signal.OnEmittedWithParam += action;
		}
	}
}
