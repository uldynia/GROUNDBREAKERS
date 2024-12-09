using Mirror;
using UnityEngine;

public class PunchTool : PowerTool
{
    [SyncVar] public float cooldown = 0;
    public GameObject p_PunchEffect;
    private void Update()
    {
        cooldown += Time.deltaTime;
    }
    public override void OnDrag()
    {
        base.OnDrag();
        Punch(transform.position, button.deltaNormalized);
    }
    [Command]
    public void Punch(Vector3 position, Vector3 delta)
    {
        if(cooldown > 0.2 && delta.sqrMagnitude > 0.1f)
        {
            cooldown = 0;
            var go = Instantiate(p_PunchEffect, position, Quaternion.identity);
            go.GetComponent<PunchEffect>().init(delta);
            NetworkServer.Spawn(go);
        }
    }
}
