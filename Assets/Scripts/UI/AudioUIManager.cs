using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIManager : MonoBehaviour
{
    private AudioManager audioManager=>AudioManager.instance;
    private byte[] audioResult;
    [SerializeField] private ChatUI chatUi;
    [SerializeField] private Button startRecordBtn;
    [SerializeField] private Button stopRecordBtn;
    [SerializeField] private Button replayRecordBtn;
    [SerializeField] private Button sendVoiceData;
    [SerializeField] private Text notiText;
    // Start is called before the first frame update
    void Start()
    {
        notiText.text = "";
        startRecordBtn.onClick.AddListener(() =>
        {
            audioManager.OnRecording((noti) =>
            {
                notiText.text = noti;
            });
        });
        stopRecordBtn.onClick.AddListener(() =>
        {
            audioManager.OnEndRecording((rs) =>
            {
                audioResult = rs;
                notiText.text = "";
            });
        });
        replayRecordBtn.onClick.AddListener((() =>
        {
           audioManager.Replay();
        }));
        sendVoiceData.onClick.AddListener((() =>
        {
            if (audioResult != null)
            {
                ServerService.Instance.SendAudio(chatUi.TargetID,audioResult);
                notiText.text = "Send audio data success";
            }
            else
            {
                notiText.text = "There no audio data to send";
            }
        }));
    }

}
