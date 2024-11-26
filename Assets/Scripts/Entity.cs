using Mirror;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    [SyncVar(hook = "dmg")] public float health;
    public float maxHealth = 100;
    public enum TYPE {
        PLAYER, ENEMY
    }
    public TYPE type;
    public System.Action<float> onDamage, onDeath;
    private void Start()
    {
        health = maxHealth;
    }
    public float Damage(float damage)
    {
        health -= damage;
        return health;
    }
    void dmg(float f, float ff) {
        onDamage?.Invoke(health);
        if(health < 0)
        {
            onDeath?.Invoke(health);
        }
    }
}
