using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTool : PowerTool
{
    [SyncVar] public Vector3 position;
    [SyncVar] public Vector3 delta;

    void Update()
    {
        if (button != null)
        {
            position = PlayerController.instance.transform.position;
            delta = button.isHolding ? button.delta : Vector3.zero;

        }
        // TODO: Play particle fx
        if(isServer)
        {
            var objects = FindObjectsByType<FlamethrowerTool>(FindObjectsSortMode.None);
            foreach (var tool in objects)
            {
                Debug.Log($"{tool.position}, {tool.delta}");
                if (tool.delta != Vector3.zero)
                {
                    foreach(var collider in Physics2D.RaycastAll(tool.position, tool.delta, 10))
                    {
                        if (tool.gameObject == collider.collider.gameObject) continue;
                        var entity = collider.collider.GetComponent<Entity>();
                        if(entity == null ) continue;
                        if (entity.type == Entity.TYPE.PLAYER)
                            entity.Damage(5 * Time.deltaTime);
                        else
                            entity.Damage(20 * Time.deltaTime);

                    }
                }
            }
        }
    }
}
