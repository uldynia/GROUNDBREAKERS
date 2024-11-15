using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerTool : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string Name, description;
    bool isHolding;
    [SerializeField] Image joystick, handle;
    Vector2 startingPoint;
    protected Vector3 delta, deltaNormalized;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        startingPoint = eventData.position;
        joystick.transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        delta = eventData.delta - startingPoint;
        deltaNormalized = delta.normalized;
    }
    private void Update()
    {
        joystick.gameObject.SetActive(isHolding);
        if (isHolding)
        {
            handle.transform.localPosition = Vector3.ClampMagnitude(delta, 100);
        }
        update();
    }
    protected virtual void update() { }
}
