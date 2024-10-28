using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isHolding;
    public System.Action onDown, onUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        onDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        onUp?.Invoke();
    }
}
