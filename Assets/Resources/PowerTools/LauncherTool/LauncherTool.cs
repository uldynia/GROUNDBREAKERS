using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class LauncherTool : PowerTool
{
    [SerializeField] GameObject p_launcher;

    [SyncVar] float cooldown;
    private void Update()
    {
        cooldown += Time.deltaTime;
    }
    public override void OnRelease()
    {
        base.OnRelease();
        Launcher();
    }
    [Command]
    void Launcher()
    {
        if (cooldown < 10) return;
        cooldown = 0;
        var launcher = Instantiate(p_launcher, PlayerController.instance.transform.position + button.deltaNormalized, Quaternion.identity);
        launcher.GetComponent<Rigidbody2D>().linearVelocity = button.deltaNormalized * 10;
        NetworkServer.Spawn(launcher);
    }
}
