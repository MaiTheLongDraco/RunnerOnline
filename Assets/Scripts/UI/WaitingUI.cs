using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingUI : MonoBehaviour
{
	public Button ConnectBtn;
	public InputField UsernameIP;
	private TCPClientChat socketClient=>ClientManager.instance.tCPClientChat;
	private void Start()
	{
		ConnectBtn.onClick.RemoveAllListeners();
		UsernameIP.onEndEdit.RemoveAllListeners();
		UsernameIP.onValueChange.RemoveAllListeners();
		InitListener();
	}
	private void InitListener()
	{
			ConnectBtn.onClick.AddListener(()=> {
				if (UsernameIP.text.Length < 6) return;
				socketClient.Connect(); 
			});
			ConnectBtn.onClick.AddListener(() => { socketClient.UserName = UsernameIP.text; });
	}
}
