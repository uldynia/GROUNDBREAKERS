using Mirror;
using UnityEngine;

public class JumpPadPowerTool : NetworkBehaviour
{
    [SyncVar] public float cooldown;
    private void Update()
    {
        if (isServer) cooldown -= Time.deltaTime;

    }

    public PowerToolButton slot;
    public Sprite sprite;

    public GameObject p_JumpPad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(PowerToolButton button)
    {
        button.onDrag += OnDrag;
        button.onEndDrag += OnEndDrag;
        button.icon.sprite = sprite;
    }

    public void OnDrag(Vector3 delta)
    {

    }
    public void OnEndDrag(Vector3 delta)
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
