using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform _background;
    private RectTransform _handle;

    public Vector2 InputVector { get; private set; }

    private void Start()
    {
        _background = GetComponent<RectTransform>();
        _handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        OnPointerUp(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPosition = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            touchPosition = Vector2.ClampMagnitude(touchPosition, _background.sizeDelta.x / 2);
            _handle.anchoredPosition = touchPosition;

            InputVector = touchPosition / (_background.sizeDelta.x / 2);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }
}
