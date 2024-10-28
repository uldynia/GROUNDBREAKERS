using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]public bool isHolding;
    public System.Action onDown, onUp;
    [SerializeField] bool ShrinkOnClick = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        onDown?.Invoke();
        if(ShrinkOnClick)
            transform.localScale = Vector3.one * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        onUp?.Invoke();
        if(ShrinkOnClick)
            transform.localScale = Vector3.one;
    }
}
