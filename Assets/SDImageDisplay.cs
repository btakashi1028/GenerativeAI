using UnityEngine;
using UnityEngine.UI;
using System;

public class SDImageDisplay : MonoBehaviour
{
    public RawImage displayImage;

    public void DisplayGeneratedImage(string base64Image)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);

        displayImage.texture = texture;
    }
}
