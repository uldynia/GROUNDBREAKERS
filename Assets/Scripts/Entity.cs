using Mirror;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SyncVar] float health = 100;
    const float maxHealth = 100;

    System.Action<float> onDamage, onDeath;
    public float Damage(float damage)
    {
        health -= damage;
        onDamage.Invoke(health);
        if(health < 0)
        {
            onDeath.Invoke(health);
        }
        return health;
    }
}
