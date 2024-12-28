using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PrivateChatUI : MonoBehaviour
{
   [SerializeField] private TMP_InputField inputTextChat;
  [SerializeField] private ScrollRect chatViewScroll;
   [SerializeField] private Button sendButton;
   [SerializeField] private Button closeBtn;
   [SerializeField] private string targetID;
   [SerializeField] private TextMeshProUGUI displayName;
   [SerializeField] private TextMeshProUGUI chatDisplay;
   [SerializeField] private ClientToServerOperationCode commandType;
   [SerializeField] private int paddingFactor;
  public ServerService serverService=>ServerService.Instance;

  private void Start()
  {
      sendButton.onClick.AddListener(Send);
      closeBtn.onClick.AddListener(()=>gameObject.SetActive(false));
        // Lắng nghe khi có người dùng mới vào server
      serverService.SubscribeOperationHandler<NotifyNewPlayerDTO>(ServerToClientOperationCode.NotifyNewPlayer,OnNotiNewPlayer);
		//Lắng nghe tin nhắn private từ server
		serverService.SubscribeOperationHandler<PrivateMessageDTO>(ServerToClientOperationCode.GetMessageResponse, OnGetPrivateMessage);
  }

  public void Inject(string targetID,Vector3 mousePos,string username)
  {
	 this.targetID = targetID;
	 displayName.text = username;
	 // transform.position = mousePos;
  }
	private void OnGetPrivateMessage(PrivateMessageDTO data)
	{
		Debug.Log($"message private {data.ToString()}");
	}
	private void OnNotiNewPlayer(NotifyNewPlayerDTO data)
  {
      Debug.Log($"Sender ID {data.SenderId} sender name {data.SenderName} content {data.Content}");
        UIManager.instance.MakeNotiSlider(data.Content);
  }
    private void Enqueu(Action callBack)
    {
        MainThreadDispatcher.Instance.Enqueue(callBack);
    }
  public void AddEmojiToInputField(int emojiIndex)
  {
	  string text = $"{PaddingHorizontalText(3)} <size=150%><sprite index={emojiIndex}></size>";
	  inputTextChat.text += text;
  }
  private string PaddingHorizontalText(int numPad)
  {
	  StringBuilder stringBuilder = new StringBuilder();
	  if (numPad > 0)
	  {
		  for (int i = 0; i < numPad; i++)
		  {
			  stringBuilder.Append(" ");
		  }
	  }
	  return stringBuilder.ToString();

  }
  private void Send()
  {
		string privateMsg = $"<color=red>[PRIVATE]</color> <color=#00E9FF><link=PLAYER_NAME#{serverService.GetClientID()}>{serverService.GetClientName()}</link></color>: " +
	  $"<color=#FFFFFFFF>{inputTextChat.text}</color>";
			commandType = ClientToServerOperationCode.SendPrivateMessage;
			serverService.SendPrivate(targetID, privateMsg);
			chatDisplay.text +=$"<align=right>{privateMsg}</align>\n";
          inputTextChat.text = "";
  }
}
//<sprite index=1> <link=PLAYER_NAME>Thông Báo</link>: <color=#00E9FF>Hiện đang trong gian đoạn Võ lâm liên đấu, hôm nay từ PLAYER_NAME>234234sdfw</link>: <color=#FFFFFFFF>rtyrtyrty</color>
//< sprite name = "EmojiOne_0" > < link = PLAYER_NAME > 234234sdfw </ link >: < color =#FFFFFFFF>ryrtyry</color>
//< sprite name = "Near" > < link = PLAYER_NAME > 234234sdfw </ link >: < color =#FFFFFFFF>rtyrtyr</color>
