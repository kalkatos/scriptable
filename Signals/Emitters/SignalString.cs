using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalString", menuName = "Signals/Signal (String)", order = 2)]
	public class SignalString : TypedSignal<string>
	{
		public string DefaultValue;

		public override void Emit ()
		{
			EmitWithParam(DefaultValue);
		}

		public void EmitWithParam (int value)
		{
			EmitWithParam(value.ToString());
		}

		public void EmitWithParam (float value)
		{
			EmitWithParam(value.ToString());
		}

		public void EmitWithParam (bool value)
		{
			EmitWithParam(value.ToString());
		}
	}
}