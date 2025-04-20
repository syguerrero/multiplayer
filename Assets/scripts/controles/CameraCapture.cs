using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraCapture : MonoBehaviour
{
    public RawImage rawImagePreview;
    public GameObject capturePanel;
    public List<Renderer> targetRenderers; // Lista de renderers a los que aplicar la textura

    private WebCamTexture webcamTexture;
    private Texture2D photo;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        rawImagePreview.texture = webcamTexture;
        rawImagePreview.material.mainTexture = webcamTexture;
        webcamTexture.Play();
        capturePanel.SetActive(true);
    }

    public void CapturePhoto()
    {
        photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();

        foreach (var renderer in targetRenderers)
        {
            renderer.material.mainTexture = photo;
        }

        webcamTexture.Stop();
        capturePanel.SetActive(false);
    }
}
