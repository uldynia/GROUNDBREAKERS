using UnityEngine;
using Mirror;
public class ShadowController : NetworkBehaviour
{
    [SerializeField] Sprite[] walkingSpriteSheet;
    SpriteRenderer sr;
    [SerializeField] Entity e;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(isServer) {
            e.onDeath += OnDeath;
        }
    }
    void OnDeath(float damage) {
        NetworkServer.Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        sr.sprite = walkingSpriteSheet[(int)( (Time.time * 3) % walkingSpriteSheet.Length)];
        if(isServer) {
            
        }
    }
}
