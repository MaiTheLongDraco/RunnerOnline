using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class UploadVoiceToCDN : MonoBehaviour
{
     private string cdnUploadUrl = "https://drive.google.com/drive/folders/1639sFuYmA7CnhIGOLssvQF0scGt5Jb70?usp=sharing";
    [SerializeField] private string audioFilePath ;
    
    // Token API nếu endpoint yêu cầu xác thực (nếu không, để trống "")
    // private string apiToken = "YOUR_API_TOKEN";
    // private string apiToken = "YOUR_API_TOKEN";
    private string apiToken = "1639sFuYmA7CnhIGOLssvQF0scGt5Jb70";
[ContextMenu("Upload Audio To CDN")]
    public void Upload()
    {
        UploadMp3ToCDN(audioFilePath);
    }
    /// <summary>
    /// Phương thức upload file MP3 lên CDN
    /// </summary>
    /// <param name="filePath">Đường dẫn đầy đủ tới file MP3 trên thiết bị</param>
    public void UploadMp3ToCDN(string filePath)
    {
        if (File.Exists(filePath))
        {
            StartCoroutine(UploadMp3Coroutine(filePath));
        }
        else
        {
            Debug.LogError("File không tồn tại: " + filePath);
        }
    }

    private IEnumerator UploadMp3Coroutine(string filePath)
    {
        // Đọc file MP3 thành byte array
        byte[] fileData = File.ReadAllBytes(filePath);
        
        // Tạo form data để chứa file và thông tin kèm theo
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, Path.GetFileName(filePath), "audio/mpeg");

        // Tạo yêu cầu HTTP POST với UnityWebRequest
        using (UnityWebRequest request = UnityWebRequest.Post(cdnUploadUrl, form))
        {
            // Thêm mã thông báo API vào header nếu cần
            if (!string.IsNullOrEmpty(apiToken))
            {
                request.SetRequestHeader("Authorization", "Bearer " + apiToken);
            }

            // Thêm header cho nội dung là mp3
            request.SetRequestHeader("Content-Type", "multipart/form-data");

            // Theo dõi tiến trình tải lên
          yield return  request.SendWebRequest();

            while (!request.isDone)
            {
                Debug.Log($"Progress: {request.uploadProgress * 100}%");
                yield return null;
            }
            // Kiểm tra kết quả tải lên
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Upload thành công: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Lỗi khi upload: " + request.error);
                Debug.Log("Upload thành công: " + request.downloadHandler.text);
            }
        }
    }
}
