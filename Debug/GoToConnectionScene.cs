using UnityEngine;
using Kalkatos.Network.Unity;

namespace Kalkatos.UnityGame.Scriptable
{
	public class GoToConnectionScene : MonoBehaviour
    {
        [SerializeField] private ScreenSignal connectionScene;

        private void Start ()
        {
            if (!NetworkClient.IsConnected)
                connectionScene?.Emit();
		}
    }
}