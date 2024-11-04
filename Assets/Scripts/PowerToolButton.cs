using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerToolButton : HoldButton, IInitializePotentialDragHandler, IDragHandler
{
    public System.Action<Vector3> onDrag, onEndDrag;
    public Image icon;
    public Vector3 start;
    public Vector3 GetDelta(PointerEventData eventData) => (new Vector3(eventData.position.x, eventData.position.y) - start).normalized;
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(GetDelta(eventData));
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onEndDrag?.Invoke(GetDelta(eventData));
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        start = eventData.position;
        eventData.useDragThreshold = false;
    }
}
