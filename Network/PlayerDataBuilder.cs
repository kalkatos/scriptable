﻿// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

#if KALKATOS_NETWORK

using Kalkatos.Network.Model;
using Kalkatos.Network.Unity;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using UnityEngine;

namespace Kalkatos.UnityGame.Scriptable.Network
{
	[CreateAssetMenu(fileName = "PlayerDataBuilder", menuName = "Network/Player Data Builder")]
	public class PlayerDataBuilder : ScriptableObject
	{
		public PlayerInfoReceiver MyInfo;
		public PlayerInfoReceiver[] OtherPlayersInfo;

		public void UpdatePlayerData ()
		{
			UpdatePlayerData(NetworkClient.MyInfo, MyInfo);
		}

		public void UpdateOtherPlayersData ()
		{
			PlayerInfo myInfo = NetworkClient.MyInfo;
			if (NetworkClient.MatchInfo == null || OtherPlayersInfo == null || myInfo == null)
				return;

			int index = 0;
			foreach (var playerInfo in NetworkClient.MatchInfo.Players)
			{
				if (playerInfo.Alias == myInfo.Alias)
					continue;
				if (OtherPlayersInfo.Length <= index)
					return;
				UpdatePlayerData(playerInfo, OtherPlayersInfo[index++]);
			}
		}

		private void UpdatePlayerData (PlayerInfo playerInfo, PlayerInfoReceiver infoReceiver)
		{
			if (playerInfo == null)
				return;
			infoReceiver?.Nickname?.EmitWithParam(playerInfo.Nickname);
			if (playerInfo.CustomData == null || infoReceiver == null)
				return;
			foreach (var infoData in playerInfo.CustomData)
			{
				if (infoReceiver.OtherData == null)
					continue;
				foreach (var receiverData in infoReceiver.OtherData)
				{
					if (receiverData.Key == infoData.Key)
					{
						receiverData.Emit(infoData.Value);
						break;
					}
				}
			}
		}
	}

	[Serializable]
	public class PlayerInfoReceiver
	{
		public SignalString Nickname;
		public PlayerData[] OtherData;
	}

	[Serializable]
	public class PlayerData
	{
		public string Key;
#if ODIN_INSPECTOR
		[LabelText("Data"), HorizontalGroup(0.3f)] 
#endif
		public DataType Type;
#if ODIN_INSPECTOR
		[HorizontalGroup(0.7f), HideLabel, ShowIf(nameof(Type), DataType.Bool)]  
#endif
		public SignalBool BoolValue;
#if ODIN_INSPECTOR
		[HorizontalGroup(0.7f), HideLabel, ShowIf(nameof(Type), DataType.Int)]  
#endif
		public SignalInt IntValue;
#if ODIN_INSPECTOR
		[HorizontalGroup(0.7f), HideLabel, ShowIf(nameof(Type), DataType.Float)]  
#endif
		public SignalFloat FloatValue;
#if ODIN_INSPECTOR
		[HorizontalGroup(0.7f), HideLabel, ShowIf(nameof(Type), DataType.String)]  
#endif
		public SignalString StringValue;

		public enum DataType { Bool, Int, Float, String }

		public string Value
		{
			get
			{
				switch (Type)
				{
					case DataType.Bool:
						return (BoolValue.Value ? "1" : "0");
					case DataType.Int:
						return IntValue.Value.ToString();
					case DataType.Float:
						return FloatValue.Value.ToString();
					case DataType.String:
					default:
						return StringValue.Value;
				}
			}
		}

		public void Emit (string value) 
		{
			if (Type == DataType.Bool)
				BoolValue.EmitWithParam(value == "1");
			else if (Type == DataType.Int && int.TryParse(value, out int intValueParsed))
				IntValue.EmitWithParam(intValueParsed);
			else if (Type == DataType.Float && float.TryParse(value, out float floatValueParsed))
				FloatValue.EmitWithParam(floatValueParsed);
			else if (Type == DataType.String)
				StringValue.EmitWithParam(value);
			else
				Logger.Log("[PlayerData] Couldn't find a valid Type.");
		}
	}
}

#endif