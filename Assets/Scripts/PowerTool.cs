using Mirror;
using UnityEngine;

public class PowerTool : NetworkBehaviour
{
    [SyncVar] public float cooldown;
    [HideInInspector] public PowerToolButton slot;
    public Sprite sprite;
    public virtual void Init(PowerToolButton button)
    {
        button.onDrag += OnDrag;
        button.onEndDrag += OnEndDrag;
        button.icon.sprite = sprite;
        slot = button;
    }
    public virtual void OnDrag(Vector3 delta)
    {

    }
    public virtual void OnEndDrag(Vector3 delta)
    {
    }
}
