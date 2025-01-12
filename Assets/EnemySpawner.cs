using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Update()
    {
        if (!Takama.instance.isServer) return;

    }
}
