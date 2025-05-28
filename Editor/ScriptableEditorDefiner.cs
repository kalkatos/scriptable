// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Kalkatos.Scriptable.Unity
{
	[InitializeOnLoad]
	public class ScriptableEditorDefiner : Editor
	{
		public static readonly string[] Symbols = new string[] { "KALKATOS_SCRIPTABLE" };

		static ScriptableEditorDefiner ()
		{
			BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
			string definesString = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
			List<string> allDefines = definesString.Split(';').ToList();
			allDefines.AddRange(Symbols.Except(allDefines));
			PlayerSettings.SetScriptingDefineSymbols(
				namedBuildTarget,
				string.Join(";", allDefines.ToArray()));
		}
	}
}
