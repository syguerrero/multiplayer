using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CropSelector : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform cropBox; // Caja de recorte (UI)
    private Vector2 startPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out startPos);

        cropBox.gameObject.SetActive(true);
        cropBox.anchoredPosition = startPos;
        cropBox.sizeDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out currentPos);

        Vector2 size = currentPos - startPos;
        cropBox.anchoredPosition = startPos + size * 0.5f;
        cropBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
    }

    public void OnPointerUp(PointerEventData eventData) { }

    public Rect GetCropRect()
    {
        return new Rect(cropBox.anchoredPosition - cropBox.sizeDelta * 0.5f, cropBox.sizeDelta);
    }
}