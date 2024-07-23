using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
    [CreateAssetMenu(fileName = "NewSignalComponent", menuName = "Signals/Signal (Component)", order = 2)]
    public class SignalComponent : TypedSignal<Component>
    {
        public Component DefaultValue;

        public override void Emit ()
        {
            EmitWithParam(DefaultValue);
        }
    }
}