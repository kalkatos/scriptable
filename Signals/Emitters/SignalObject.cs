using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalObject", menuName = "Signals/Signal (Object)", order = 2)]
	public class SignalObject : TypedSignal<object>
	{
		public object DefaultValue;
		public GameObject DefaultGameObject;
		public ScriptableObject DefaultScriptable;

		private object value;

		public override object Value
		{
			get
			{
				if (value == null)
				{
					SetDefaultValue();
					value = DefaultValue;
				}
				return value;
			}
			set => this.value = value;
		}

		public override void Emit ()
		{
			SetDefaultValue();
			EmitWithParam(DefaultValue);
		}

		private void SetDefaultValue ()
		{
			if (DefaultGameObject != null)
			{
				DefaultValue = DefaultGameObject;
			}
			else if (DefaultScriptable != null)
			{
				DefaultValue = DefaultScriptable;
			}
		}
	}
}