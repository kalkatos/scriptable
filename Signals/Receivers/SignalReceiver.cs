using System;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;  
#endif

namespace Kalkatos.UnityGame.Scriptable
{
	public class SignalReceiver : MonoBehaviour
	{
		[SerializeField] private SignalReceiverBit[] receivers;

		private void Awake ()
		{
			foreach (var item in receivers)
				item.Initialize();
		}

        private void Start ()
        {
            foreach (var item in receivers)
                item.EmitOnStart();
        }

        private void OnDestroy ()
		{
			foreach (var item in receivers)
				item.Dispose();
		}
	}

	[Serializable]
	public class SignalReceiverBit
	{
#if ODIN_INSPECTOR
		[OnValueChanged(nameof(VerifySignal))] 
#endif
        [SerializeField] private Signal signal;
        [SerializeField] private bool emitOnStart;
#if ODIN_INSPECTOR
        [HideIf(nameof(isAnyOtherTypedSignal))] 
#endif
        [SerializeField] private UnityEvent action;
#if ODIN_INSPECTOR
		[ShowIf(nameof(isBoolSignal))] 
#endif
        [SerializeField] private ValueBinding<bool>[] BoolValueBindings;
#if ODIN_INSPECTOR
		[ShowIf(nameof(isIntSignal))] 
#endif
        [SerializeField] private ValueBinding<int>[] IntValueBindings;
#if ODIN_INSPECTOR
		[ShowIf(nameof(isStringSignal))] 
#endif
        [SerializeField] private ValueBinding<string>[] StringValueBindings;
#if ODIN_INSPECTOR
		[ShowIf(nameof(isFloatSignal))] 
#endif
        [SerializeField] private ValueBinding<float>[] FloatValueBindings;
		[HideInInspector, SerializeField] private bool isAnyOtherTypedSignal;
		[HideInInspector, SerializeField] private bool isBoolSignal;
		[HideInInspector, SerializeField] private bool isIntSignal;
		[HideInInspector, SerializeField] private bool isStringSignal;
		[HideInInspector, SerializeField] private bool isFloatSignal;

        public void Initialize ()
		{
			if (signal is TypedSignal<bool>)
				((TypedSignal<bool>)signal).OnSignalEmittedWithParam.AddListener(HandleBoolSignalEmitted);
			else if (signal is TypedSignal<int>)
				((TypedSignal<int>)signal).OnSignalEmittedWithParam.AddListener(HandleIntSignalEmitted);
			else if (signal is TypedSignal<string>)
				((TypedSignal<string>)signal).OnSignalEmittedWithParam.AddListener(HandleStringSignalEmitted);
			else if (signal is TypedSignal<float>)
				((TypedSignal<float>)signal).OnSignalEmittedWithParam.AddListener(HandleFloatSignalEmitted);
			else
				signal.OnSignalEmitted.AddListener(HandleSignalEmitted);
		}

		public void Dispose ()
		{
			if (signal is TypedSignal<bool>)
				((TypedSignal<bool>)signal)?.OnSignalEmittedWithParam.RemoveListener(HandleBoolSignalEmitted);
			else if (signal is TypedSignal<int>)
				((TypedSignal<int>)signal)?.OnSignalEmittedWithParam.RemoveListener(HandleIntSignalEmitted);
			else if (signal is TypedSignal<string>)
				((TypedSignal<string>)signal)?.OnSignalEmittedWithParam.RemoveListener(HandleStringSignalEmitted);
			else if (signal is TypedSignal<float>)
				((TypedSignal<float>)signal).OnSignalEmittedWithParam.RemoveListener(HandleFloatSignalEmitted);
			else
				signal?.OnSignalEmitted.RemoveListener(HandleSignalEmitted);
		}

		public void EmitOnStart ()
		{
			if (!emitOnStart)
				return;
            if (signal is TypedSignal<bool>)
                HandleBoolSignalEmitted(((TypedSignal<bool>)signal).Value);
            else if (signal is TypedSignal<int>)
                HandleIntSignalEmitted(((TypedSignal<int>)signal).Value);
            else if (signal is TypedSignal<string>)
                HandleStringSignalEmitted(((TypedSignal<string>)signal).Value);
            else if (signal is TypedSignal<float>)
                HandleFloatSignalEmitted(((TypedSignal<float>)signal).Value);
            else
                HandleSignalEmitted();
        }

		private void VerifySignal ()
		{
			isAnyOtherTypedSignal = false;
			isBoolSignal = signal != null && signal is TypedSignal<bool>;
			isIntSignal = signal != null && signal is TypedSignal<int>;
			isStringSignal = signal != null && signal is TypedSignal<string>;
			isFloatSignal = signal != null && signal is TypedSignal<float>;
			isAnyOtherTypedSignal = isBoolSignal || isIntSignal || isStringSignal || isFloatSignal;
		}

		private void HandleSignalEmitted ()
		{
			action?.Invoke();
		}

		private void HandleBoolSignalEmitted (bool b)
		{
			foreach (var item in BoolValueBindings)
				item.TreatValue(b);
		}

		private void HandleIntSignalEmitted (int value)
		{
			foreach (var item in IntValueBindings)
				item.TreatValue(value);
		}

		private void HandleStringSignalEmitted (string value)
		{
			foreach (var item in StringValueBindings)
				item.TreatValue(value);
		}

		private void HandleFloatSignalEmitted (float value)
		{
			foreach (var item in FloatValueBindings)
				item.TreatValue(value);
		}
	}
}