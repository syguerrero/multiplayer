// ControlDeTexturas.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class ImageUploader : MonoBehaviour
{
    public RawImage rawImagePreview;
    public GameObject panelCarga;
    public GameObject sliders;
    public List<Renderer> targetRenderers;
    public Texture2D imagenPorDefecto;
    public CropSelector cropSelector;
    public GameObject botonRecortar; // Botón que se debe ocultar

    private Texture2D imagenSeleccionada;
    private Texture2D imagenOriginal;

#if UNITY_EDITOR
    public void SubirImagen()
    {
        sliders.SetActive(true);
        string path = UnityEditor.EditorUtility.OpenFilePanel("Selecciona una imagen", "", "png,jpg,jpeg");

        if (!string.IsNullOrEmpty(path))
        {
            StartCoroutine(CargarImagen(path));
        }
    }
#endif

    IEnumerator CargarImagen(string path)
    {
        byte[] datos = File.ReadAllBytes(path);
        Texture2D original = new Texture2D(2, 2);
        original.LoadImage(datos);

        imagenSeleccionada = original;
        imagenOriginal = original;

        if (rawImagePreview != null)
            rawImagePreview.texture = original;

        if (botonRecortar != null)
            botonRecortar.SetActive(true);

        yield return null;
    }

    public void UsarImagenPorDefecto()
    {
        imagenSeleccionada = imagenPorDefecto;
        imagenOriginal = imagenPorDefecto;

        if (rawImagePreview != null && imagenPorDefecto != null)
            rawImagePreview.texture = imagenPorDefecto;

        if (botonRecortar != null)
            botonRecortar.SetActive(true);
    }

    public void RecortarYAplicar()
    {
        if (imagenOriginal == null)
        {
            Debug.LogWarning("No hay imagen original cargada.");
            return;
        }

        Rect cropRectUI = cropSelector.GetCropRect();
        RectTransform rawRect = rawImagePreview.rectTransform;

        float widthRatio = imagenOriginal.width / rawRect.rect.width;
        float heightRatio = imagenOriginal.height / rawRect.rect.height;

        int x = Mathf.Clamp((int)((cropRectUI.x + rawRect.rect.width / 2f) * widthRatio), 0, imagenOriginal.width - 1);
        int y = Mathf.Clamp((int)((cropRectUI.y + rawRect.rect.height / 2f) * heightRatio), 0, imagenOriginal.height - 1);
        int w = Mathf.Clamp((int)(cropRectUI.width * widthRatio), 1, imagenOriginal.width - x);
        int h = Mathf.Clamp((int)(cropRectUI.height * heightRatio), 1, imagenOriginal.height - y);

        Texture2D recorte = new Texture2D(w, h);
        recorte.SetPixels(imagenOriginal.GetPixels(x, y, w, h));
        recorte.Apply();

        foreach (var rend in targetRenderers)
        {
            rend.material.mainTexture = recorte;
        }

        rawImagePreview.texture = recorte;
        panelCarga.SetActive(false);

        if (botonRecortar != null)
            botonRecortar.SetActive(false);

        SceneManager.LoadScene(1);
    }

    public void CerrarSinAplicar()
    {
        panelCarga.SetActive(false);
    }
}

