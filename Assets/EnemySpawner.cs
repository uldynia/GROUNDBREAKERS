using Mirror;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public int numberOfEnemies = 5;
    bool isActive = true;
    public ChanceTableItem[] enemies;
    [System.Serializable]
    public class ChanceTableItem
    {
        public float chance; // between 0 and 100
        public GameObject enemy;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if(collision.CompareTag("Player") && isActive)
        {
            isActive = false;
            for (int i = 0; i < numberOfEnemies; i++)
            {
                float rand = Random.Range(0f, 100f);
                foreach (var enemy in enemies)
                {
                    if ( (rand -= enemy.chance) < 0)
                    {
                        // instantiate enemy
                        var go = Instantiate(enemy.enemy, transform.position + Vector3.right * Random.Range(-1f, 1f), Quaternion.identity);
                        NetworkServer.Spawn(go);
                    }
                }
            }
        }
    }
}
