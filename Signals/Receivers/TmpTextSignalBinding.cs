#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using TMPro;
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
	public class TmpTextSignalBinding : MonoBehaviour
	{
		[SerializeField] private SignalBase signal;
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
			switch (signal)
			{
				case TypedSignal<string> stringSignal:
					stringSignal.OnEmittedWithParam += UpdateText;
					break;
				case TypedSignal<bool> boolSignal:
					boolSignal.OnEmittedWithParam += UpdateText;
					break;
				case TypedSignal<float> floatSignal:
					floatSignal.OnEmittedWithParam += UpdateText;
					break;
				case TypedSignal<int> intSignal:
					intSignal.OnEmittedWithParam += UpdateText;
					break;
				default:
					Logger.LogError($"Signal {signal.name} is from a type not implemented.");
					return;
			}
		}

		private void OnDisable ()
		{
			switch (signal)
			{
				case TypedSignal<string> stringSignal:
					stringSignal.OnEmittedWithParam -= UpdateText;
					break;
				case TypedSignal<bool> boolSignal:
					boolSignal.OnEmittedWithParam -= UpdateText;
					break;
				case TypedSignal<float> floatSignal:
					floatSignal.OnEmittedWithParam -= UpdateText;
					break;
				case TypedSignal<int> intSignal:
					intSignal.OnEmittedWithParam -= UpdateText;
					break;
				default:
					return;
			}
		}

		private void UpdateText (string text)
		{
			textComponent?.SetText(text);
			textComponentInputField?.SetTextWithoutNotify(text);
		}

		private void UpdateText (int number)
		{
			textComponent?.SetText(number.ToString());
			textComponentInputField?.SetTextWithoutNotify(number.ToString());
		}

		private void UpdateText (float number)
		{
			textComponent?.SetText(number.ToString());
			textComponentInputField?.SetTextWithoutNotify(number.ToString());
		}

		private void UpdateText (bool b)
		{
			textComponent?.SetText(b.ToString());
			textComponentInputField?.SetTextWithoutNotify(b.ToString());
		}
	}
}