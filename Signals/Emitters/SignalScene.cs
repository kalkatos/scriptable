using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewScreenSignal", menuName = "Signals/Signal (Screen)", order = 3)]
	public class SignalScene : SignalBase
	{
		public bool LoadAsync;

		public override void Emit ()
		{
			OnSignalEmitted?.Invoke();
			if (LoadAsync)
				SceneManager.LoadSceneAsync(name);
			else
				SceneManager.LoadScene(name);
		}
	}
}