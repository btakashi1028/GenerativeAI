using System.Collections;
using UnityEngine;

public class SDStableDiffusionManager : MonoBehaviour
{
    public SDScreenshotManager screenshotManager;
    public SDImageConverter imageConverter;
    public SDStableDiffusionAPI stableDiffusionAPI;
    public SDImageDisplay imageDisplay;
    public SDImageSaver imageSaver;

    void Start()
    {
        stableDiffusionAPI.stableDiffusionManager = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Start Generate Image From Screenshot");
            GenerateImageFromScreenshot();
        }
    }

    public void GenerateImageFromScreenshot()
    {
        string screenshotPath = Application.persistentDataPath + "/screenshot.png";
        
        // Capture screenshot
        screenshotManager.CaptureScreenshot();

        // Start coroutine to process after a delay (to allow the screenshot to be saved)
        StartCoroutine(ProcessScreenshot(screenshotPath));
    }

    private IEnumerator ProcessScreenshot(string screenshotPath)
    {
        yield return new WaitForSeconds(1);  // Wait to ensure screenshot is saved

        // Convert to byte array
        byte[] imageBytes = imageConverter.ConvertScreenshotToByteArray(screenshotPath);

        // Send to Stable Diffusion API and handle response
        yield return stableDiffusionAPI.GenerateImage(imageBytes);

        // APIから取得した生成画像のBase64データを保存
        if (stableDiffusionAPI.generatedBase64Image != null)
        {
            imageSaver.SaveGeneratedImage(stableDiffusionAPI.generatedBase64Image);
        }
    }

    // 画像を表示
    public void DisplayGeneratedImage(string base64Image)
    {
        imageDisplay.DisplayGeneratedImage(base64Image);
    }
}