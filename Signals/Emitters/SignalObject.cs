using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalObject", menuName = "Signals/Signal (Object)", order = 2)]
	public class SignalObject : TypedSignal<object>
	{
		public object DefaultValue;

		public override void Emit ()
		{
			EmitWithParam(DefaultValue);
		}
	}
}