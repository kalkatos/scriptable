using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalScriptableObject", menuName = "Signals/Signal (ScriptableObject)", order = 7)]
	public class SignalScriptableObject : TypedSignal<ScriptableObject>
	{
		public ScriptableObject DefaultValue;

		public override void Emit ()
		{
			EmitWithParam(DefaultValue);
		}
	}
}