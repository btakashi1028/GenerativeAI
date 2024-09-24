using UnityEngine;

public class SDScreenshotManager : MonoBehaviour
{
    public void CaptureScreenshot()
    {
        string screenshotPath = Application.persistentDataPath + "/screenshot.png";
        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log("Screenshot saved");
    }
}
