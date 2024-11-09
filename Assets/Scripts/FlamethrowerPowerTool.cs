using Mirror;
using UnityEngine;

public class FlamethrowerPowerTool : NetworkBehaviour
{
    [SerializeField] GameObject p_flamethrower;
    float holdDuration = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    [Command]
    public void Flamethrow(Vector3 direction, float holdTime)
    {

        if(holdTime > 0.2f)
        {
            var enemies = Physics.RaycastAll(transform.position, direction, 3, LayerMask.NameToLayer("Entity"));
        }
            
    }
}
