using System.Collections;
using Mirror;
using UnityEngine;

public class ExoticFlintKnock : PowerTool
{
    [SyncVar] public float cooldown;
    [SerializeField] GameObject p_bullet;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] sprites;
    [SyncVar] public int sprite;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnRelease()
    {
        base.OnRelease();
        if (cooldown < 0)
            StartCoroutine(StartShoot(button.deltaNormalized));
    }
    bool isShooting;
    IEnumerator StartShoot(Vector3 delta)
    {
        if (!isShooting)
        {
            isShooting = true;
            float duration = 0;
            sr.transform.right = delta;
            while ((duration += Time.fixedDeltaTime) < 1)
            {
                sprite = Mathf.RoundToInt(duration * 8);
                GrapplingHook.instance.points.Clear();
                rb.AddForce(-rb.linearVelocity * 10);
                yield return new WaitForFixedUpdate();
            }
            sprite = 0;
            Shoot(transform.position, delta);
        }
    }
    private void Update()
    {
        sr.sprite = sprites[Mathf.Min(sprite, sprites.Length - 1)];
        if (isServer)
        {
            cooldown -= Time.deltaTime;
        }
    }
    [Command]
    public void Shoot(Vector3 position, Vector3 delta, NetworkConnectionToClient sender = null)
    {
        var go = Instantiate(p_bullet, position, Quaternion.identity).GetComponent<FlintKnockBullet>();
        go.Init(delta * 30);
        NetworkServer.Spawn(go.gameObject);
        foreach (var owned in sender.owned)
        {
            if(owned.TryGetComponent<ExoticFlintKnock>(out var e)) {
                e.Recoil(sender, delta * -30);
                break;
            }
        }
    }
    [TargetRpc]
    public void Recoil(NetworkConnectionToClient target, Vector3 delta)
    {
        GrapplingHook.instance.points.Clear();
        rb.linearVelocity = delta;
        isShooting = false;
    }
}
