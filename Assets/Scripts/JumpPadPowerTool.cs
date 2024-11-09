using Mirror;
using UnityEngine;

public class JumpPadPowerTool : PowerTool
{
    public GameObject p_JumpPad;
    private void Update()
    {
        if (isServer) cooldown -= Time.deltaTime;

    }

    public override void OnEndDrag(Vector3 delta)
    {
        SpawnJumpPad(delta);
    }

    [Command]
    public void SpawnJumpPad(Vector3 delta)
    {
        if(cooldown < 0)
        {
            cooldown = 0;
            var jumpPad = Instantiate(p_JumpPad, transform.position + delta, Quaternion.identity);
            jumpPad.GetComponent<Rigidbody2D>().AddForce(delta * 10, ForceMode2D.Impulse);;
            NetworkServer.Spawn(jumpPad);
        }
    }
}
