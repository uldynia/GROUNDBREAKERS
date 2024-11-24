using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerToolButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    PowerTool tool;

    public bool isHolding;
    [SerializeField] Image joystick, handle, icon;
    Vector2 startingPoint;
    public Vector3 delta, deltaNormalized;

    public void Init(PowerTool _tool)
    {
        tool = _tool;
        icon.sprite = tool.icon;
        tool.button = this;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        startingPoint = eventData.position;
        joystick.transform.position = eventData.position;
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        delta = eventData.position - startingPoint;
        deltaNormalized = delta.normalized;
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        tool.OnRelease();
    }
    private void Update()
    {
        joystick.gameObject.SetActive(isHolding);
        if (isHolding)
        {
            handle.transform.localPosition = Vector3.ClampMagnitude(delta, 50);
            tool.OnDrag();
        }
        update();
    }
    protected virtual void update() { }

}
