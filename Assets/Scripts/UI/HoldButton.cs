using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler
{
    public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
}