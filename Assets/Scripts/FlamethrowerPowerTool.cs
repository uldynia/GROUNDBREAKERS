using Mirror;
using UnityEngine;

public class FlamethrowerPowerTool : PowerTool
{
    float holdDuration = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (slot.isHolding) {
            holdDuration += Time.deltaTime;
            Flamethrow(slot.GetDelta(), holdDuration);
        }

    }
    [Command]
    public void Flamethrow(Vector3 direction, float holdTime)
    {
        Debug.Log(direction);
        if(holdTime > 0.2f)
        {
            var enemies = Physics.RaycastAll(transform.position, direction, 3, LayerMask.NameToLayer("Entity"));
        }
            
    }
}
