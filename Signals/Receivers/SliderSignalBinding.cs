using UnityEngine;
using UnityEngine.UI;

namespace Kalkatos.UnityGame.Scriptable
{
    public class SliderSignalBinding : MonoBehaviour
    {
		[SerializeField] private TypedSignal<float> signal;
		[SerializeField] private Slider slider;

        private void Reset ()
        {
			if (slider == null)
				slider = GetComponent<Slider>();
        }

        private void OnEnable ()
        {
			UpdateSlider(signal.Value);
            slider.onValueChanged.AddListener(signal.EmitWithParam);
        }

        private void OnDisable ()
        {
            slider.onValueChanged.RemoveListener(signal.EmitWithParam);
        }

        private void UpdateSlider (float value)
        {
			slider.value = value;
        }
	}
}