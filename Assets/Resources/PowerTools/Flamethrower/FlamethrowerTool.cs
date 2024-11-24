using Mirror;
using UnityEngine;

public class FlamethrowerTool : PowerTool
{
    [SyncVar] public Vector3 position;
    [SyncVar] public Vector3 delta;
    private void Start()
    {
        RegisterFlamethrowers();
    }

    FlamethrowerTool[] tools;
    [Command]
    void RegisterFlamethrowers()
    {
        tools = FindObjectsByType<FlamethrowerTool>(FindObjectsSortMode.None);
    }
    void Update()
    {
        position = PlayerController.instance.transform.position;
        delta = button.deltaNormalized;
        // TODO: Play particle fx
        if(isServer)
        {
            foreach (var tool in tools) {
                if(tool.delta != Vector3.zero)
                {
                    // TODO: raycast to entity
                }
            }
        }
    }
}
