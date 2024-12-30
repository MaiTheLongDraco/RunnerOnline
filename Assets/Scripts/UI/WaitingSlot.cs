using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
public class WaitingSlot : MonoBehaviour
{
    public string slotName;
	public string slotId;
    public TextMeshProUGUI dislayText;
    public Image playerImage;
	public Button ReadyButton;
	public bool IsReady;
	public bool IsAvaiable;
	private void Awake()
	{
		IsAvaiable=true;
		dislayText.gameObject.SetActive(false);
		playerImage.gameObject.SetActive(false);
		ReadyButton.interactable=false;
		ReadyButton.onClick.AddListener(OnReadyButtonClick);
	}
	private void Start()
	{
		ServerService.Instance.SubscribeOperationHandler<SyncAllPlayerDTO>(ServerToClientOperationCode.SyncAllPlayer, OnUpdatePlayerState);

	}
	private void OnUpdatePlayerState(SyncAllPlayerDTO data)
	{
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			Debug.Log($" get sync player ");
			var currentSlot = data.AllPLayer.Find(slot => slot.Id.Equals(slotId));
			if (!currentSlot.Equals(default(PlayerState)))
			{
					var txt = currentSlot.IsReady ? "OK" : "Ready";
					ReadyButton.GetComponentInChildren<Text>().text = txt;
				IsReady = currentSlot.IsReady;
			}
		});
		
	}

	private void OnReadyButtonClick()
	{
		IsReady=!IsReady;
		PlayerState playerState = new PlayerState() { 
			Id = slotId,
			playerName = slotName,
			IsReady = IsReady,
			Position= Vector3.zero,
		};
		List<PlayerState> playerStates = new List<PlayerState>(){ playerState };
		SyncAllPlayerDTO syncPlayerDTO = new SyncAllPlayerDTO()
		{
			AllPLayer = playerStates
		};
		ServerService.Instance.SendSyncAllPlayer(syncPlayerDTO,ClientToServerOperationCode.SyncAllPlayer);
	}

	public void Init(string slotName)
    {
        this.slotName = slotName;
		IsAvaiable = false;
		dislayText.text = slotName;
		dislayText.gameObject.SetActive(true);
		playerImage.gameObject.SetActive(true);
		ReadyButton.interactable = true;
	}
	public void SetButtonText(PlayerState playerState)
	{
		var txt = playerState.IsReady ? "OK" : "Ready";
		ReadyButton.GetComponentInChildren<Text>().text = txt;
	}
	public void DisActive()
	{
		IsAvaiable = true;
		dislayText.text = string.Empty;
		dislayText.gameObject.SetActive(false);
		playerImage.gameObject.SetActive(false);
		ReadyButton.interactable = false;
	}
}
