using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTool : PowerTool
{
    [SyncVar] public Vector3 position;
    [SyncVar] public Vector3 delta;
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (delta != Vector3.zero)
        {
            var swag = ps.emission;
            swag.rateOverTime = 50;
            var swag2 = ps.shape;
            swag2.rotation = new Vector3(0, 0,
                Vector2.SignedAngle(Vector2.right, delta)
            ) ;
        }
        else
        {
            var swag = ps.emission;
            swag.rateOverTime = 0;
        }

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
