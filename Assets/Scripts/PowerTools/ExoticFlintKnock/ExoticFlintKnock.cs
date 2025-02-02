using System.Collections;
using Mirror;
using UnityEngine;

public class ExoticFlintKnock : PowerTool
{
    [SyncVar] public float cooldown;
    [SerializeField] GameObject p_bullet; 
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
            
            while ((duration += Time.fixedDeltaTime) < 1)
            {
                GrapplingHook.instance.points.Clear();
                rb.AddForce(-rb.linearVelocity * 10);
                yield return new WaitForFixedUpdate();
            }
            Shoot(transform.position, delta);
        }
    }
    private void Update()
    {
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
        ExoticFlintKnock e;
        foreach(var owned in sender.owned)
        {
            if(owned.TryGetComponent<ExoticFlintKnock>(out e)) {
                e.Recoil(delta * -30);
                break;
            }
        }
    }
    [ClientRpc]
    public void Recoil(Vector3 delta)
    {
        GrapplingHook.instance.points.Clear();
        rb.linearVelocity = delta;
        isShooting = false;
    }
}
