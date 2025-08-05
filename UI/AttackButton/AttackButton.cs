using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPointerDown = false;
    public PlayerController controller;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    void Update()
    {
        if (_isPointerDown)
        {
            controller.StartAttack();
        }
    }
}
