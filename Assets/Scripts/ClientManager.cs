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
	public Player playerPrefab;
	ServerService service=>ServerService.Instance;
	private void Start()
	{
		instance = this;
		tCPClientChat = GetComponentInChildren<TCPClientChat>();
		tCPClientChat.OnConnectSuccess += OnConnect;
		ListenCallBack();
	}

	private void OnConnect()
	{
		service.SendSyncAllPlayer(new SyncAllPlayerDTO() { AllPLayer= new List<PlayerState>()},ClientToServerOperationCode.SyncAllPlayer);
	}

	private void ListenCallBack()
	{
		service.SubscribeOperationHandler<SyncAllPlayerDTO>(ServerToClientOperationCode.SyncAllPlayer, OnSyncAllPlayer);
	}
	private void OnSyncAllPlayer(SyncAllPlayerDTO dto)
	{
		playerStates = dto.AllPLayer;
		StringBuilder sb = new StringBuilder();	
		foreach (PlayerState player in playerStates) { 
			sb.AppendLine($" Player ID {player.Id}");
			if(player.Id!=tCPClientChat.clientID)
			{
				CreatePlayer(player.Id, player.Position);
			}
		}
		Debug.Log(sb.ToString());
	}
	private  void CreatePlayer(string id,Vector3 pos)
	{
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			var player = Instantiate(playerPrefab, pos, Quaternion.identity);
			player.Id = id;
		});
		
	}
}
public struct PlayerState
{
	public string Id;
	public Vector3 Position;
}
public struct SyncAllPlayerDTO
{
	public List<PlayerState> AllPLayer;
}

