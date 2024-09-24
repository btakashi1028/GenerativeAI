using System.IO;
using UnityEngine;

public class SDImageConverter : MonoBehaviour
{
    public byte[] ConvertScreenshotToByteArray(string path)
    {
        byte[] imageBytes = File.ReadAllBytes(path);
        return imageBytes;
    }
}
