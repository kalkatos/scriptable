using System.Collections;
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable
{
    [DefaultExecutionOrder(-1)]
    public class StorageSignalBinding : MonoBehaviour
    {
        private static StorageSignalBinding instance;

        public SaveBinding[] Bindings;

        private void Awake ()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            InitializeBindings();
        }

        private void OnDestroy ()
        {
            DisposeBindings();
        }

        private void InitializeBindings ()
        {
            for (int i = 0; i < Bindings.Length; i++)
            {
                var bind = Bindings[i];
                bind.Initialize(this);
                switch (bind.Signal)
                {
                    case SignalBool signalBool:
                        bool defaultBool = bool.TryParse(bind.DefaultValue, out bool parsedBool) ? parsedBool : false;
                        bool boolValue = Storage.Load(bind.Key, defaultBool.ToString()) == true.ToString();
                        signalBool.Value = boolValue;
                        signalBool.DefaultValue = boolValue;
                        break;
                    case SignalString signalString:
                        string stringValue = Storage.Load(bind.Key, bind.DefaultValue);
                        signalString.Value = stringValue;
                        signalString.DefaultValue = stringValue;
                        break;
                    case SignalInt signalInt:
                        int defaultInt = int.TryParse(bind.DefaultValue, out int parsedInt) ? parsedInt : 0;
                        int intValue = Storage.Load(bind.Key, defaultInt);
                        signalInt.Value = intValue;
                        signalInt.DefaultValue = intValue;
                        break;
                    case SignalFloat signalFloat:
                        float defaultFloat = float.TryParse(bind.DefaultValue, out float parsedFloat) ? parsedFloat : 0f;
                        float floatValue = Storage.Load(bind.Key, defaultFloat);
                        signalFloat.Value = floatValue;
                        signalFloat.DefaultValue = floatValue;
                        break;
                    default:
                        Logger.LogWarning($"Signal binding {bind.Signal} is not a TypedSignal.");
                        break;
                }
                bind.Subscribe();
            }
        }

        private void DisposeBindings ()
        {
            for (int i = 0; i < Bindings.Length; i++)
                Bindings[i].Unsubscribe();
        }
    }

    [System.Serializable]
    public class SaveBinding
    {
        public string Key;
        public string DefaultValue;
        public Signal Signal;

        private MonoBehaviour mono;
        private WaitForSeconds waitHalfSecond = new WaitForSeconds(0.5f);
        private float lastSave;
        private Coroutine scheduledSaveCoroutine;
        private bool? boolSaveScheduled;
        private string stringSaveScheduled;
        private int? intSaveScheduled;
        private float? floatSaveScheduled;

        public void Initialize (MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public void Subscribe ()
        {
            switch (Signal)
            {
                case TypedSignal<bool> signalBool:
					signalBool.OnSignalEmittedWithParam.AddListener(OnBoolValueChanged);
                    break;
                case TypedSignal<string> signalString:
					signalString.OnSignalEmittedWithParam.AddListener(OnStringValueChanged);
                    break;
                case TypedSignal<int> signalInt:
					signalInt.OnSignalEmittedWithParam.AddListener(OnIntValueChanged);
                    break;
                case TypedSignal<float> signalFloat:
					signalFloat.OnSignalEmittedWithParam.AddListener(OnFloatValueChanged);
                    break;
            }
        }

        public void Unsubscribe ()
        {
            SaveAll();
            switch (Signal)
            {
                case TypedSignal<bool> signalBool:
					signalBool.OnSignalEmittedWithParam.RemoveListener(OnBoolValueChanged);
                    break;
                case TypedSignal<string> singalString:
					singalString.OnSignalEmittedWithParam.RemoveListener(OnStringValueChanged);
                    break;
                case TypedSignal<int> signalInt:
					signalInt.OnSignalEmittedWithParam.RemoveListener(OnIntValueChanged);
                    break;
                case TypedSignal<float> signalFloat:
					signalFloat.OnSignalEmittedWithParam.RemoveListener(OnFloatValueChanged);
                    break;
            }
        }

        private void OnBoolValueChanged (bool b)
        {
            if (Time.time - lastSave > 0.5f)
            {
                Storage.Save(Key, b.ToString());
                lastSave = Time.time;
                return;
            }
            boolSaveScheduled = b;
            ScheduleSave();
        }

        private void OnStringValueChanged (string s)
        {
            if (Time.time - lastSave > 0.5f)
            {
                Storage.Save(Key, s);
                lastSave = Time.time;
                return;
            }
            stringSaveScheduled = s;
            ScheduleSave();
        }

        private void OnIntValueChanged (int i)
        {
            if (Time.time - lastSave > 0.5f)
            {
                Storage.Save(Key, i);
                lastSave = Time.time;
                return;
            }
            intSaveScheduled = i;
            ScheduleSave();
        }

        private void OnFloatValueChanged (float f)
        {
            if (Time.time - lastSave > 0.5f)
            {
                Storage.Save(Key, f);
                lastSave = Time.time;
                return;
            }
            floatSaveScheduled = f;
            ScheduleSave();
        }

        private void ScheduleSave ()
        {
            if (scheduledSaveCoroutine != null)
                return;
            scheduledSaveCoroutine = mono.StartCoroutine(WaitAndSave());
        }

        private IEnumerator WaitAndSave ()
        {
            yield return waitHalfSecond;
            SaveAll();
            scheduledSaveCoroutine = null;
        }

        private void SaveAll ()
        {
            if (boolSaveScheduled.HasValue)
            {
                Storage.Save(Key, boolSaveScheduled.Value.ToString());
                boolSaveScheduled = null;
            }
            if (stringSaveScheduled != null)
            {
                Storage.Save(Key, stringSaveScheduled);
                stringSaveScheduled = null;
            }
            if (intSaveScheduled.HasValue)
            {
                Storage.Save(Key, intSaveScheduled.Value);
                intSaveScheduled = null;
            }
            if (floatSaveScheduled.HasValue)
            {
                Storage.Save(Key, floatSaveScheduled.Value);
                floatSaveScheduled = null;
            }
        }
    }
}