#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using TMPro;
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	public class TmpTextSignalBinding : MonoBehaviour
	{
		[SerializeField] private TypedSignal<string> signal;
#if ODIN_INSPECTOR
		[HideIf(nameof(textComponentInputField))]
#endif
		[SerializeField] private TMP_Text textComponent;
#if ODIN_INSPECTOR
		[HideIf(nameof(textComponent))]
#endif
		[SerializeField] private TMP_InputField textComponentInputField;

		private void Reset ()
		{
			if (textComponent == null)
				textComponent = GetComponent<TMP_Text>();
			if (textComponentInputField == null)
				textComponentInputField = GetComponent<TMP_InputField>();
		}

		private void OnEnable ()
		{
			signal.OnSignalEmittedWithParam.AddListener(UpdateText);
			UpdateText(signal.Value);
		}

		private void OnDisable ()
		{
			signal.OnSignalEmittedWithParam.RemoveListener(UpdateText);
		}

		private void UpdateText (string text)
		{
			textComponent?.SetText(text);
			textComponentInputField?.SetTextWithoutNotify(text);
		}
	}
}