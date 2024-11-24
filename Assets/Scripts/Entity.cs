using Mirror;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SyncVar] float health = 100;
    const float maxHealth = 100;
    public enum TYPE {
        PLAYER, ENEMY
    }
    public TYPE type;
    System.Action<float> onDamage, onDeath;
    public float Damage(float damage)
    {
        health -= damage;
        Debug.Log($"HP: {health}, damage: {damage}");
        onDamage?.Invoke(health);
        if(health < 0)
        {
            onDeath?.Invoke(health);
        }
        return health;
    }
}
