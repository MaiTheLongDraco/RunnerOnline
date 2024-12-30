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
	[SerializeField] private NotifylSlider notifylSlider;

	public List<WaitingSlot> WaitingSlots { get => waitingSlots; set => waitingSlots = value; }
    public List<PlayerState> playerStates;
	// Start is called before the first frame update
	void Start()
    {
		notifylSlider.gameObject.SetActive(false);
		GetAllSlot();
        startGameButton.onClick.AddListener(OnStartButtonClick);
	}

	private void OnStartButtonClick()
	{
		var condition = waitingSlots.Any(slot => !slot.IsReady&&slot.IsAvaiable==false);
        if (condition)
        {
			notifylSlider.Inject($" Có người chơi chưa sẵn sàng");
			notifylSlider.gameObject.SetActive(true);

			Debug.Log($" Có người chơi chưa sẵn sàng");
			return;
        }
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
				slot.SetButtonText(data);
				PlayerState playerState = new PlayerState() { 
					Id = data.Id,
					IsReady = data.IsReady,
					playerName = data.playerName,
					Position = data.Position
				};
				if(!playerStates.Contains(playerState) )
				{
					playerStates.Add(playerState);
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
