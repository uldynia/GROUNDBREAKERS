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
        if (isServer)
        {
            e.onDeath += OnDeath;
        }
    }
    void OnDeath(float damage)
    {
        NetworkServer.Destroy(gameObject);
    }

    public enum STATE
    {
        IDLE,
        PATROL,
        CHASE
    }
    public STATE state;
    float stateDuration, direction, jumpCooldown;
    void Update()
    {
        if (!isServer) return;
        stateDuration += Time.deltaTime;
        jumpCooldown += Time.deltaTime;
        switch (state)
        {
            case STATE.IDLE:
                sr.sprite = walkingSpriteSheet[0];
                if (stateDuration > 1)
                {
                    state = STATE.PATROL;
                    stateDuration = 0;
                    direction = Random.Range(-1, 1) == -1 ? -1 : 1;
                }
                break;
            case STATE.PATROL:
                sr.sprite = walkingSpriteSheet[(int)((Time.time * 3) % walkingSpriteSheet.Length)];
                var diff = new Vector3(direction, 0, 0);

                if (jumpCooldown > 2 && Physics2D.Raycast(transform.position, diff, 3, LayerMask.NameToLayer("Terrain")))
                {
                    jumpCooldown = 0;
                    GetComponent<Rigidbody2D>().AddForce(Vector3.up * 5);
                }
                transform.position += diff * Time.deltaTime * 4;

                if (stateDuration > 1.5f)
                {
                    state = STATE.IDLE;
                    stateDuration = 0;
                }
                break;
            case STATE.CHASE:

                break;
        }
    }
}
