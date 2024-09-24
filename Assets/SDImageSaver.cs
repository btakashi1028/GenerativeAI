using System;
using System.IO;
using UnityEngine;

public class SDImageSaver : MonoBehaviour
{
    public void SaveGeneratedImage(string base64Image)
    {
        // Base64文字列をバイト配列に変換
        byte[] imageBytes = Convert.FromBase64String(base64Image);

        // 保存先のパスを設定
        string filePath = Application.persistentDataPath + "/generatedImage.png";

        try
        {
            // ファイルにバイト配列を書き込む
            File.WriteAllBytes(filePath, imageBytes);
            Debug.Log("Image saved");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save image: " + e.Message);
        }
    }
}
