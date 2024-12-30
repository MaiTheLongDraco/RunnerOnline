using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
   public SocketClient socketClient;
	public TCPClientChat tCPClientChat;
	public static ClientManager instance;
	public List<PlayerState> playerStates = new List<PlayerState>();
	public bool IsClientConnect => tCPClientChat.IsClientConnect();
	ServerService service=>ServerService.Instance;
	private void Start()
	{
		instance = this;
		tCPClientChat = GetComponentInChildren<TCPClientChat>();
		tCPClientChat.OnConnectSuccess += OnConnect;
		ListenCallBack();
	}
	public void AddPlayerState(PlayerState state)
	{
		if(playerStates.Contains(state)) return;
		playerStates.Add(state);
	}
	public bool IsExistPlayer(string id)
	{
		foreach (PlayerState player in playerStates) {
			if(player.Id.Equals(id)) return true;
		}
		return false;
	}

	private void OnConnect()
	{
		//PlayerState player = new PlayerState()
		//{
		//	Id = tCPClientChat.clientID,
		//	playerName = tCPClientChat.name,
		//	Position = new Vector3()
		//};
		//List<PlayerState> playerStates = new List<PlayerState>();
		//playerStates.Add(player);
		//service.SendSyncAllPlayer(new SyncAllPlayerDTO() { AllPLayer = playerStates }, ClientToServerOperationCode.SyncAllPlayer);
	}

	private void ListenCallBack()
	{
		//service.SubscribeOperationHandler<SyncAllPlayerDTO>(ServerToClientOperationCode.SyncAllPlayer, OnSyncAllPlayer);
		//service.SubscribeOperationHandler<ClientIdDto>(ServerToClientOperationCode.UpdatePlayerId, OnGetPlayerId);
	}
	//private void OnSyncAllPlayer(SyncAllPlayerDTO dto)
	//{
	//	playerStates = dto.AllPLayer;
	//	StringBuilder sb = new StringBuilder();	
	//	foreach (PlayerState player in playerStates) { 
	//		sb.AppendLine($" Player ID {player.Id}");
	//		if(player.Id!=tCPClientChat.clientID)
	//		{
	//			//CreatePlayer(player.Id, player.Position);
	//		}
	//	}
	//	Debug.Log(sb.ToString());
	//}
	
}
[Serializable]
public struct PlayerState
{
	public string Id;
	public string playerName;
	public bool IsReady;
	[JsonIgnore]
	public Vector3 Position;
}
public struct SyncAllPlayerDTO
{
	public List<PlayerState> AllPLayer;
}

