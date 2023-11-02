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
                    case TypedSignal<bool>:
                        ((TypedSignal<bool>)bind.Signal).Value = Storage.Load(bind.Key, 0) == 1;
                        break;
                    case TypedSignal<string>:
                        ((TypedSignal<string>)bind.Signal).Value = Storage.Load(bind.Key, "");
                        break;
                    case TypedSignal<int>:
                        ((TypedSignal<int>)bind.Signal).Value = Storage.Load(bind.Key, 0);
                        break;
                    case TypedSignal<float>:
                        ((TypedSignal<float>)bind.Signal).Value = Storage.Load(bind.Key, 0f);
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
                case TypedSignal<bool>:
                    ((TypedSignal<bool>)Signal).OnSignalEmittedWithParam.AddListener(OnBoolValueChanged);
                    break;
                case TypedSignal<string>:
                    ((TypedSignal<string>)Signal).OnSignalEmittedWithParam.AddListener(OnStringValueChanged);
                    break;
                case TypedSignal<int>:
                    ((TypedSignal<int>)Signal).OnSignalEmittedWithParam.AddListener(OnIntValueChanged);
                    break;
                case TypedSignal<float>:
                    ((TypedSignal<float>)Signal).OnSignalEmittedWithParam.AddListener(OnFloatValueChanged);
                    break;
            }
        }

        public void Unsubscribe ()
        {
            SaveAll();
            switch (Signal)
            {
                case TypedSignal<bool>:
                    ((TypedSignal<bool>)Signal).OnSignalEmittedWithParam.RemoveListener(OnBoolValueChanged);
                    break;
                case TypedSignal<string>:
                    ((TypedSignal<string>)Signal).OnSignalEmittedWithParam.RemoveListener(OnStringValueChanged);
                    break;
                case TypedSignal<int>:
                    ((TypedSignal<int>)Signal).OnSignalEmittedWithParam.RemoveListener(OnIntValueChanged);
                    break;
                case TypedSignal<float>:
                    ((TypedSignal<float>)Signal).OnSignalEmittedWithParam.RemoveListener(OnFloatValueChanged);
                    break;
            }
        }

        private void OnBoolValueChanged (bool b)
        {
            if (Time.time - lastSave > 0.5f)
            {
                Storage.Save(Key, b ? 1 : 0);
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
                Storage.Save(Key, boolSaveScheduled.Value ? 1 : 0);
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