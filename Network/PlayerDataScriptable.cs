// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

#if KALKATOS_NETWORK

#if ODIN_INSPECTOR
using Sirenix.OdinInspector; 
#endif
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable.Network
{
	[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Network/Player Data")]
	public class PlayerDataScriptable : ScriptableObject
	{
#if ODIN_INSPECTOR
		[InlineProperty, HideLabel]  
#endif
        public PlayerData Data;
	}
}

#endif