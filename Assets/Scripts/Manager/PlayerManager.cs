using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]private List<Player> m_Players ;
    [SerializeField]private ServerService serverService ;
	private void Start()
	{
		serverService=ServerService.Instance;
		m_Players = new List<Player>();
		serverService.SubscribeOperationHandler<PlayerInput>(ServerToClientOperationCode.UpdatePlayerPos, OnUpdatePlayersPos);
	}

	private void OnUpdatePlayersPos(PlayerInput data)
	{
		foreach (Player player in m_Players) { 
			if(data.PlayerID==player.Id)
			{

			}
		}
	}

	public void AddPlayer(Player player)
	{
		if(m_Players.Contains(player))return;
		m_Players.Add(player);
	}
}
