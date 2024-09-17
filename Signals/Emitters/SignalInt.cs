using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalInt", menuName = "Signals/Signal (Int)", order = 5)]
	public class SignalInt : TypedSignal<int>
	{
		public int DefaultValue;

		public override void Emit ()
		{
			EmitWithParam(DefaultValue);
		}

		public void ParseString (string value)
		{
			if (int.TryParse(value, out int parsed))
			{
				EmitWithParam(parsed);
			}
		}

		public void EmitWithChildIndex (Transform child)
		{
			EmitWithParam(child.GetSiblingIndex());
		}
	}
}