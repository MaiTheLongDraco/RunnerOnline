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
	public List<WaitingSlot> WaitingSlots { get => waitingSlots; set => waitingSlots = value; }

	// Start is called before the first frame update
	void Start()
    {
        GetAllSlot();
        startGameButton.onClick.AddListener(OnStartButtonClick);
	}

	private void OnStartButtonClick()
	{
		this.gameObject.SetActive(false);
        foreach (var slot in waitingSlots) {
            slot.DisActive();
        }
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
