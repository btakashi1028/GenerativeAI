using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SDStableDiffusionAPI : MonoBehaviour
{
    private string apiUrl = "https://api.stability.ai/v1/generation/stable-diffusion-v1-6/image-to-image";  // Example endpoint
    private string apiKey = "sk-NzDvESjy4p855b0JLDCl1hDwXR0T3JCGV9RGrCwxDSL37BDz";
    public SDStableDiffusionManager stableDiffusionManager;
    [HideInInspector]public string generatedBase64Image;

    public IEnumerator GenerateImage(byte[] imageBytes)
    {
        // multipart/form-data のためのフォームデータを作成
        WWWForm form = new WWWForm();

        // テキストプロンプトを追加
        // form.AddField("text_prompts[0][text]", "monochrome, grayscale");//qiitaで載せたやつ
        // form.AddField("text_prompts[0][text]", "scarlet blue snow, ice, flowers, lunar arc, eternity, diamond dust, dedication, previous life, finite life, infinite wreckage, call to the beyond");
        //form.AddField("text_prompts[0][text]", "Graffiti Art Style, wildstyle, colorful");
        form.AddField("text_prompts[0][text]", "ink sketch, flowers, japanese, chinese");
        form.AddField("text_prompts[0][weight]", "1.0");

        // 初期画像を追加 (バイナリ形式の画像データ)
        form.AddBinaryData("init_image", imageBytes, "image.png", "image/png");

        // init_image_mode と image_strength を指定
        form.AddField("init_image_mode", "IMAGE_STRENGTH");
        form.AddField("image_strength", "0.15");//qiitaで載せたやつは0.25、scarletBlueは0.25、graffitiは0.15

        // その他のパラメータを追加
        form.AddField("cfg_scale", "7");
        // 'height' と 'width' を削除
        // form.AddField("height", "512");
        // form.AddField("width", "512");
        form.AddField("sampler", "K_DPMPP_2M");
        form.AddField("samples", "1");
        form.AddField("steps", "30");

        // HTTPリクエストを作成
        UnityWebRequest request = UnityWebRequest.Post(apiUrl, form);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // リクエスト送信
        yield return request.SendWebRequest();

        // エラーハンドリング
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Error Body: " + request.downloadHandler.text);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Response: " + jsonResponse);

            // レスポンスのパース
            StableDiffusionResponse response = JsonConvert.DeserializeObject<StableDiffusionResponse>(jsonResponse);

            if (response.artifacts != null && response.artifacts.Count > 0)
            {
                string base64Image = response.artifacts[0].base64;
                generatedBase64Image = base64Image;
                // Managerに渡して表示
                stableDiffusionManager.DisplayGeneratedImage(generatedBase64Image);
            }
        }
    }
}

[System.Serializable]
public class StableDiffusionResponse
{
    public List<Artifact> artifacts;
}

[System.Serializable]
public class Artifact
{
    public string base64;  // Assuming 'base64' is the field name
}
