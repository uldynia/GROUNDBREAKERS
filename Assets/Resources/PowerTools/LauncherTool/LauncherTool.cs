using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class LauncherTool : PowerTool
{
    [SerializeField] GameObject p_launcher;

    [SyncVar] float cooldown;
    protected override void update()
    {
        base.update();
        cooldown += Time.deltaTime;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Launcher();
    }
    [Command]
    void Launcher()
    {

        if (cooldown < 10) return;
        cooldown = 0;
        var launcher = Instantiate(p_launcher, PlayerController.instance.transform.position + deltaNormalized, Quaternion.identity);
        launcher.GetComponent<Rigidbody2D>().linearVelocity = deltaNormalized * 10;
        NetworkServer.Spawn(launcher);
    }
}
