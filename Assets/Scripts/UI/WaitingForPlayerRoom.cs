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

   private void GetAllSlot()
    {
		WaitingSlots=transform.GetChild(0).GetComponentsInChildren<WaitingSlot>().ToList();
        for (int i = 0; i < WaitingSlots.Count; i++) {
            WaitingSlots[i].playerImage.sprite=playerIcon[i];
        }
	}
}
