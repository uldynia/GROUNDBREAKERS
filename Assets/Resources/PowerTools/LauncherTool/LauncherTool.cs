using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class LauncherTool : PowerTool
{
    [SyncVar] float cooldown;
    private void Update()
    {
        cooldown += Time.deltaTime;
    }
    public override void OnRelease()
    {
        base.OnRelease();
        Launcher(button.deltaNormalized, PlayerController.instance.transform.position);
    }
    [Command]
    void Launcher(Vector3 delta, Vector3 pos)
    {
        if (cooldown < 10) return;
        cooldown = 0;
        GameObject jumpPad = Resources.Load<GameObject>("PowerTools/LauncherTool/JumpPad");
        var launcher = Instantiate(jumpPad, pos + delta, Quaternion.identity);
        launcher.GetComponent<Rigidbody2D>().linearVelocity = delta * 10;
        NetworkServer.Spawn(launcher);
    }
}
