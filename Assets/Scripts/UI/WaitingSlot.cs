using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WaitingSlot : MonoBehaviour
{
    public string slotName;
	public string slotId;
    public TextMeshProUGUI dislayText;
    public Image playerImage;
	public bool IsAvaiable;
	private void Awake()
	{
		IsAvaiable=true;
		dislayText.gameObject.SetActive(false);
		playerImage.gameObject.SetActive(false);
	}
	public void Init(string slotName)
    {
        this.slotName = slotName;
		IsAvaiable = false;
		dislayText.text = slotName;
		dislayText.gameObject.SetActive(true);
		playerImage.gameObject.SetActive(true);

	}
	public void DisActive()
	{
		IsAvaiable = true;
		dislayText.text = string.Empty;
		dislayText.gameObject.SetActive(false);
		playerImage.gameObject.SetActive(false);
	}
}
