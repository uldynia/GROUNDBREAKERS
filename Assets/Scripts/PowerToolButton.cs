using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerToolButton : HoldButton, IInitializePotentialDragHandler, IDragHandler
{
    public System.Action<Vector3> onDrag, onEndDrag;
    public Image icon;
    public Vector3 start;
    [SerializeField] GameObject joystick, joysticktop; // preview skill direction
    public Vector3 GetDelta(PointerEventData eventData) => (new Vector3(eventData.position.x, eventData.position.y) - start).normalized;
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(GetDelta(eventData));
        joysticktop.transform.localPosition = GetDelta(eventData) * 20;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onEndDrag?.Invoke(GetDelta(eventData));
        joystick.SetActive(false);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        start = eventData.position;
        eventData.useDragThreshold = false;
        joystick.SetActive(true);
        joystick.transform.position = eventData.position;
    }
}
