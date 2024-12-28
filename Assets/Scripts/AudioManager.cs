using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioPlayer audioPlayer; 
    [SerializeField] private AudioRecorder audioRecorder;
    [SerializeField] private AudioSource audioSource;
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
        audioPlayer = GetComponentInChildren<AudioPlayer>();
        audioRecorder = GetComponentInChildren<AudioRecorder>();
    }

    public void OnRecording(UnityAction<string> cb)
    {
        StartRecording();
        cb?.Invoke($"Đang ghi âm ....");
    }

    public void OnEndRecording(UnityAction<byte[]> cb)
    {
        StopRecording(cb);
    }

    public void Replay()
    {
        var data = GetAudioResult();
        audioSource.PlayOneShot(data);
    }
    public void StartRecording()
    {
        audioRecorder.StartRecording();
    }

    public AudioClip GetAudioResult()
    {
        if (audioRecorder.Result != null) return audioRecorder.Result;
        else
        {
            return null;
        }
    }
    public void StopRecording(UnityAction<byte[]> cb)
    {
        audioRecorder.StopRecording();
        var clip = audioRecorder.Result;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        var byteData = ConvertSamplesToByteArray(samples);
        cb?.Invoke(byteData);
    }
    // IEnumerator RecordAndSendAudio(int duration)
    // {
    //     UpdateChatDisplay("Bắt đầu ghi âm...");
    //     // Ghi âm từ microphone mặc định
    //     AudioClip clip = Microphone.Start(null, false, duration, 44100);
    //     // Chờ ghi âm xong
    //     yield return new WaitForSeconds(duration);
    //     Microphone.End(null);
    //     UpdateChatDisplay("Ghi âm hoàn tất. Đang gửi...");
    //
    //     // Chuyển đổi AudioClip thành byte array
    //     float[] samples = new float[clip.samples * clip.channels];
    //     clip.GetData(samples, 0);
    //     byte[] byteArray = ConvertSamplesToByteArray(samples);
    //
    //     // Encode byte array thành base64 string
    //     string base64Audio = Convert.ToBase64String(byteArray);
    //
    //     // Tạo ProtocolMessage để gửi âm thanh
    //     var protocolMessage = new ProtocolMessage
    //     {
    //         ProtocolType = (int)ClientToServerOperationCode.SendAudio,
    //         Data = base64Audio
    //     };
    //     string json = JsonConvert.SerializeObject(protocolMessage) + "\n";
    //     byte[] buffer = Encoding.UTF8.GetBytes(json);
    //     stream.Write(buffer, 0, buffer.Length);
    //
    //     UpdateChatDisplay("Âm thanh đã được gửi.");
    // }

    byte[] ConvertSamplesToByteArray(float[] samples)
    {
        byte[] byteArray = new byte[samples.Length * 4];
        Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);
        return byteArray;
    }
}
