using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitingForPlayerRoom : MonoBehaviour
{
    [SerializeField]private List<WaitingSlot> waitingSlots;
    [SerializeField] private List<Sprite> playerIcon;

	public List<WaitingSlot> WaitingSlots { get => waitingSlots; set => waitingSlots = value; }

	// Start is called before the first frame update
	void Start()
    {
        GetAllSlot();

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
			foreach (var data in syncAllPlayerDTO.AllPLayer)
			{
                Debug.Log($"Player id {data.Id}");
                Debug.Log($"Player name {data.playerName}");
                if(data.Id!=ClientManager.instance.tCPClientChat.clientID)
                {
					var slot = FindAvaiableSLot();
                    slot.Init(data.playerName);
					slot.slotId=data.Id;
				}
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
