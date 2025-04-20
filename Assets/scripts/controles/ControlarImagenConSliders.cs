using UnityEngine;
using UnityEngine.UI;

public class ControlarImagenConSliders : MonoBehaviour
{
    public RectTransform rawImageTransform;
    public Slider sliderPosY;
    public Slider sliderPosX;
    public Slider sliderScale;

    void Start()
    {
        // Opcional: conectar valores iniciales
        sliderPosX.onValueChanged.AddListener(MoverHorizontal);
        sliderPosY.onValueChanged.AddListener(MoverVertical);
        sliderScale.onValueChanged.AddListener(Zoom);
    }

    public void MoverHorizontal(float valor)
    {
        rawImageTransform.anchoredPosition = new Vector2(valor, rawImageTransform.anchoredPosition.y);
    }

    public void MoverVertical(float valor)
    {
        rawImageTransform.anchoredPosition = new Vector2(rawImageTransform.anchoredPosition.x, valor);
    }

    public void Zoom(float valor)
    {
        rawImageTransform.localScale = new Vector3(valor, valor, 1f);
    }
}
