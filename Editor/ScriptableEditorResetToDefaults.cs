// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using Kalkatos.UnityGame.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Kalkatos.Scriptable.Unity
{
    public class ScriptableEditorResetToDefaults : Editor
    {
        private static T[] FindAssets<T> () where T : Object
        { 
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            var assets = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]); 
                assets[i] = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            } 
            return assets; 
        }

        [InitializeOnLoadMethod]
        private static void ResetAllTypedSignals ()
        {
            SignalBool[] allBoolSignals = FindAssets<SignalBool>();
            if (allBoolSignals != null)
                foreach (var item in allBoolSignals)
                    item.Value = item.DefaultValue;
            SignalString[] allStringSignals = FindAssets<SignalString>();
            if (allStringSignals != null)
                foreach (var item in allStringSignals)
                    item.Value = item.DefaultValue;
            SignalInt[] allIntSignals = FindAssets<SignalInt>();
            if (allIntSignals != null)
                foreach (var item in allIntSignals)
                    item.Value = item.DefaultValue; 
            SignalFloat[] allFloatSignals = FindAssets<SignalFloat>();
            if (allFloatSignals != null)
                foreach (var item in allFloatSignals)
                    item.Value = item.DefaultValue;
            ScreenSignal[] allScreenSignals = FindAssets<ScreenSignal>();
            if (allScreenSignals != null)
                foreach (var item in allScreenSignals)
                    item.Value = false;
            SignalState[] allStateSignals = FindAssets<SignalState>();
            if (allStateSignals != null)
                foreach (var item in allStateSignals)
                    item.Value = "";
        }
    }
}
