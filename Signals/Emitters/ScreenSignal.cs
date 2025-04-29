using UnityEngine;
using UnityEngine.SceneManagement;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;  
#endif

namespace Kalkatos.UnityGame.Scriptable
{
	[CreateAssetMenu(fileName = "NewScreenSignal", menuName = "Signals/Signal (Screen)", order = 3)]
	public class ScreenSignal : TypedSignal<bool>
	{
		public bool IsScene;
#if ODIN_INSPECTOR
		[ShowIf(nameof(IsScene))] 
#endif
		public bool LoadAsync;

		public override void Emit ()
		{
			base.EmitWithParam(true);
			if (IsScene)
			{
				if (LoadAsync)
					SceneManager.LoadSceneAsync(name);
				else
					SceneManager.LoadScene(name);
			}
		}

		public override void EmitWithParam (bool param)
		{
			base.EmitWithParam(param);
			if (IsScene)
			{
				if (param)
				{
					if (LoadAsync)
						SceneManager.LoadSceneAsync(name);
					else
						SceneManager.LoadScene(name);
				}
				else
					SceneManager.UnloadSceneAsync(name);
			}
		}
	}
}