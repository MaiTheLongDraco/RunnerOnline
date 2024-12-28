using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
   [SerializeField] private TMP_InputField inputTextChat;
  [SerializeField] private ScrollRect chatViewScroll;
   [SerializeField] private Button sendButton;
   [SerializeField] private string targetID;
   [SerializeField] private Text displayName;
   [SerializeField] private TextMeshProUGUI chatDisplay;
   [SerializeField] private ClientToServerOperationCode commandType;
   [SerializeField] private int paddingFactor;
   [SerializeField] private LinkHandlerTMPText m_LinkHandlerTMPText;

   public string TargetID
   {
	   get { return targetID; }
   }
  public ServerService serverService=>ServerService.Instance;

  private void Start()
  {
	  m_LinkHandlerTMPText = GetComponentInChildren<LinkHandlerTMPText>();
	  m_LinkHandlerTMPText.Inject(OnClickOnLinkText);
      sendButton.onClick.AddListener(Send);
        //Lắng nghe tin nhắn public từ server
      serverService.SubscribeOperationHandler<PublicMessageDTO>(ServerToClientOperationCode.MessageReceived,OnGetPublicMessage);
        // Lắng nghe khi có người dùng mới vào server
      serverService.SubscribeOperationHandler<NotifyNewPlayerDTO>(ServerToClientOperationCode.NotifyNewPlayer,OnNotiNewPlayer);
        // Lắng nghe khi connect thành công
      serverService.SubscribeOperationHandler<ClientIdDto>(ServerToClientOperationCode.UpdatePlayerId,OnConnectSuccess);
		//Lắng nghe tin nhắn private từ server
		serverService.SubscribeOperationHandler<PrivateMessageDTO>(ServerToClientOperationCode.GetMessageResponse, OnGetPrivateMessage);
  }

  private void OnClickOnLinkText(TMP_LinkInfo linkInfo,Vector3 mousePos)
  {
	  // Lấy ID hoặc thông tin link mà bạn muốn
	  string linkID = linkInfo.GetLinkID();
	  string linkText = linkInfo.GetLinkText();Debug.Log("Double-clicked link with ID: " + linkID);
	  string[] charSplit = linkID.Split("#");
	  if(charSplit[1]==serverService.GetClientID())return;
	  targetID = charSplit[1];
	  UIManager.instance.GetPrivateChatUI().Inject(targetID,mousePos,linkText);
	  UIManager.instance.GetPrivateChatUI().gameObject.SetActive(true);
	  Debug.Log("Link Text: " + linkText+$"char split {charSplit[1]}");
	  targetID = String.Empty;
  }

  private void OnGetPublicMessage(PublicMessageDTO data)
    {
        Debug.Log($"message broadcast {data.ToString()}");
        Enqueu(() =>
        {
			chatDisplay.text += data.Content + "\n";
        });
  }
	private void OnGetPrivateMessage(PrivateMessageDTO data)
	{
		Enqueu(() =>
		{
			chatDisplay.text += data.Content + "\n";
		});
		Debug.Log($"message private {data.ToString()} target ID {data.TargetID}");
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
  private void OnConnectSuccess(ClientIdDto data)
  {
      MainThreadDispatcher.Instance.Enqueue(() =>
      {
          displayName.text = serverService.GetClientName();
      });
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
        string newMessgage = $" <color=#00E9FF><link=PLAYER_NAME#{serverService.GetClientID()}>{serverService.GetClientName()}</link></color>: " +
        $"<color=#FFFFFFFF>{inputTextChat.text}</color>";
		string privateMsg = $"<color=red>[PRIVATE]</color> <color=#00E9FF><link=PLAYER_NAME#{serverService.GetClientID()}>{serverService.GetClientName()}</link></color>: " +
	  $"<color=#FFFFFFFF>{inputTextChat.text}</color>";
		bool isPrivate = string.Empty == targetID ? false : true;
          if (isPrivate)
          {
			commandType = ClientToServerOperationCode.SendPrivateMessage;
			serverService.SendPrivate(targetID, privateMsg);
			chatDisplay.text +=$"<align=right>{privateMsg}</align>\n";

          }
          else
          {
            commandType = ClientToServerOperationCode.SendMessage;
              serverService.SendPublic(newMessgage, commandType);
              chatDisplay.text += $"<align=right>{newMessgage}</align>\n";
          }
          inputTextChat.text = "";
  }
}
//<sprite index=1> <link=PLAYER_NAME>Thông Báo</link>: <color=#00E9FF>Hiện đang trong gian đoạn Võ lâm liên đấu, hôm nay từ PLAYER_NAME>234234sdfw</link>: <color=#FFFFFFFF>rtyrtyrty</color>
//< sprite name = "EmojiOne_0" > < link = PLAYER_NAME > 234234sdfw </ link >: < color =#FFFFFFFF>ryrtyry</color>
//< sprite name = "Near" > < link = PLAYER_NAME > 234234sdfw </ link >: < color =#FFFFFFFF>rtyrtyr</color>
