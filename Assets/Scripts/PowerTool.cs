using Mirror;
using UnityEngine;

public class PowerTool : NetworkBehaviour
{
    public bool selectable = false;
    public string description;
    public Sprite icon;
    public PowerToolButton button;
    public virtual void OnDrag() { }
    public virtual void OnRelease() { }
}
