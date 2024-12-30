using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaitingForPlayerRoom : MonoBehaviour
{
    [SerializeField]private List<WaitingSlot> waitingSlots;
    [SerializeField] private List<Sprite> playerIcon;
    [SerializeField] private Button startGameButton;
	public Player playerPrefab;
	[SerializeField] private PlayerManager playerMnTran;

	public List<WaitingSlot> WaitingSlots { get => waitingSlots; set => waitingSlots = value; }
    public List<PlayerState> playerStates=> ClientManager.instance.playerStates;
	// Start is called before the first frame update
	void Start()
    {
		GetAllSlot();
        startGameButton.onClick.AddListener(OnStartButtonClick);
	}

	private void OnStartButtonClick()
	{
		this.gameObject.SetActive(false);
        foreach (var playerState in waitingSlots) {
            if(!playerState.IsAvaiable)
            {
				CreatePlayer(playerState);
			}
		}
		foreach (var slot in waitingSlots)
		{
			slot.DisActive();
		}
	}
	private void CreatePlayer(WaitingSlot state)
	{
			var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playerMnTran.transform);
			player.Init(state.slotId, state.slotName);
        player.SetRender(state.playerImage.sprite);
		playerMnTran.AddPlayer(player);
	}

	public void OnNotifyNewUser(NotifyNewPlayerDTO notifyNewPlayerDTO)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            foreach (var slot in waitingSlots) { 
                if(slot.IsAvaiable)
                {
                    slot.Init(notifyNewPlayerDTO.SenderName);
                    slot.slotId = notifyNewPlayerDTO.SenderId;
                    break;
                }
            }
        });
    }
    public void OnSyncAllPlayer(SyncAllPlayerDTO syncAllPlayerDTO) {
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			foreach (var slot1 in waitingSlots)
			{
				slot1.DisActive();
			}
			foreach (var data in syncAllPlayerDTO.AllPLayer)
			{
                Debug.Log($"Player id {data.Id}");
                Debug.Log($"Player name {data.playerName}");
				var slot = FindAvaiableSLot();
				slot.Init(data.playerName);
				slot.slotId = data.Id;
                slot.IsAvaiable = false;
			}
		});
	}
    private WaitingSlot FindAvaiableSLot()
    {
        foreach (var slot in waitingSlots)
        {
            if(slot.IsAvaiable)return slot;
        }
        return null;
    }
	private void GetAllSlot()
    {
		WaitingSlots=transform.GetChild(0).GetComponentsInChildren<WaitingSlot>().ToList();
        for (int i = 0; i < WaitingSlots.Count; i++) {
            WaitingSlots[i].playerImage.sprite=playerIcon[i];
        }
	}
}
