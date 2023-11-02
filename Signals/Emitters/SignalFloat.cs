using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewSignalFloat", menuName = "Signals/Signal (Float)", order = 6)]
	public class SignalFloat : TypedSignal<float>
	{
		public float DefaultValue;

		private float lastLogTime;

		public override void Emit ()
		{
			EmitWithParam(DefaultValue);
		}

		protected override void Log ()
		{
			if (Time.time - lastLogTime > 1)
			{
				base.Log();
				lastLogTime = Time.time;
			}
		}
	}
}