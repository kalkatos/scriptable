#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignal", menuName = "Signals/Signal ()", order = 0)]
	public class Signal : ScriptableObject
	{
		public UnityEvent OnSignalEmitted;

		public virtual void Emit ()
		{
			Log();
			OnSignalEmitted?.Invoke();
		}

		protected virtual void Log ()
		{
			if (SignalsSettings.Instance.EmitDebug)
				Logger.Log($"[Signal] {name} emitted.");
		}

#if ODIN_INSPECTOR
		[Button("Emit"), PropertyOrder(99)]
#endif
		protected void Emit_Debug ()
		{
			Emit();
		}
	}

	public abstract class TypedSignal<T> : Signal, IValueGetter<T>
	{
		public UnityEvent<T> OnSignalEmittedWithParam;

#if ODIN_INSPECTOR
		[PropertyOrder(1)]
#endif
		public T Value;
		[Space(10)]
#if ODIN_INSPECTOR
		[PropertyOrder(5)]
#endif
		[SerializeField] private ValueBinding<T>[] ValueBindings;

		public override void Emit ()
		{
			Log();
			OnSignalEmitted?.Invoke();
			OnSignalEmittedWithParam?.Invoke(Value);
			if (ValueBindings != null)
				foreach (var item in ValueBindings)
					item.TreatValue(Value);
		}

		public virtual void EmitWithParam (T param)
		{
			Value = param;
			Log();
			OnSignalEmittedWithParam?.Invoke(param);
			if (ValueBindings != null)
				foreach (var item in ValueBindings)
					item.TreatValue(param);
		}

		public T GetValue () => Value;

		protected override void Log ()
		{
			if (SignalsSettings.Instance.EmitDebug)
				Logger.Log($"[{GetType().Name}] {name} emitted. Value = {Value}");
		}

#if ODIN_INSPECTOR
		[Button("Emit With Param"), PropertyOrder(99)]
#endif
		protected void EmitWithParam_Debug ()
		{
			EmitWithParam(GetValue());
		}
	}

	[Serializable]
	public class ValueBinding<T>
	{
#if ODIN_INSPECTOR
		[HorizontalGroup(0.7f), LabelText("Condition")]
#endif
		public Equality Equality;
#if ODIN_INSPECTOR
		[HorizontalGroup, HideLabel, HideIf(nameof(Equality), Equality.Any)]
#endif
		public T ExpectedValue;
		public UnityEvent<T> Event;

		public void TreatValue (T value)
		{
			if (Equality == Equality.Any
				|| (Equality == Equality.Equals && (ReferenceEquals(ExpectedValue, value) || value.Equals(ExpectedValue)))
				|| (Equality == Equality.NotEquals && !ReferenceEquals(ExpectedValue, value) && !value.Equals(ExpectedValue))
				|| (Equality == Equality.GreaterThan &&
					((typeof(T) == typeof(int) && int.Parse(value.ToString()) > int.Parse(ExpectedValue.ToString()))
					|| (typeof(T) == typeof(float) && float.Parse(value.ToString()) > float.Parse(ExpectedValue.ToString()))))
				|| (Equality == Equality.LessThan &&
					((typeof(T) == typeof(int) && int.Parse(value.ToString()) < int.Parse(ExpectedValue.ToString()))
					|| (typeof(T) == typeof(float) && float.Parse(value.ToString()) < float.Parse(ExpectedValue.ToString()))))
				|| (Equality == Equality.GreaterThanOrEquals &&
					((typeof(T) == typeof(int) && int.Parse(value.ToString()) >= int.Parse(ExpectedValue.ToString()))
					|| (typeof(T) == typeof(float) && float.Parse(value.ToString()) >= float.Parse(ExpectedValue.ToString()))))
				|| (Equality == Equality.LessThanOrEquals &&
					((typeof(T) == typeof(int) && int.Parse(value.ToString()) <= int.Parse(ExpectedValue.ToString()))
					|| (typeof(T) == typeof(float) && float.Parse(value.ToString()) <= float.Parse(ExpectedValue.ToString()))))
				)
				Event?.Invoke(value);
		}
	}

	public enum Equality
	{
		Any,
		Equals,
		NotEquals,
		GreaterThan,
		LessThan,
		GreaterThanOrEquals,
		LessThanOrEquals,
	}
}