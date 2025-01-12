using UnityEngine;
using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
public class ShadowController : NetworkBehaviour
{
    [SerializeField] Sprite[] walkingSpriteSheet;
    SpriteRenderer sr;
    [SerializeField] float damage = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isServer)
        {
            GetComponent<Entity>().onDeath += OnDeath;
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
        CHASE,
        CHARGE
    }
    public STATE state;
    [SyncVar] float stateDuration, direction, jumpCooldown;
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
                Route();

                if (stateDuration > 1.5f)
                {
                    state = STATE.IDLE;
                    stateDuration = 0;
                }
                break;
            case STATE.CHASE:
                // route toward first aggro'd player
                direction = Mathf.Clamp((players[0].transform.position.x - transform.position.x), -1, 1);
                if (Mathf.Abs(players[0].transform.position.x - transform.position.x) > 1)
                    Route();
                else
                {
                    stateDuration = 0;
                    state = STATE.CHARGE;
                }
                break;
            case STATE.CHARGE:
                if (stateDuration > 1f)
                {
                    var hits = Physics2D.RaycastAll(transform.position, new Vector2(direction, 0));
                    foreach (var hit in hits)
                    {
                        if(hit.collider.CompareTag("Player"))
                        {
                            hit.collider.GetComponent<Entity>().Damage(damage);
                        }
                    }

                    state = STATE.CHASE;
                    stateDuration = 0;
                }
                break;
        }
    }
    List<PlayerController> players = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if(collision.CompareTag("Player"))
        {
            players.Add(collision.GetComponent<PlayerController>());
            state = STATE.CHASE;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isServer) return;
        if (collision.CompareTag("Player"))
        {
            players.Remove(collision.GetComponent<PlayerController>());
            if(players.Count == 0)
            {
                state = STATE.PATROL;
                stateDuration = 0;
            }
        }
    }

    void Route()
    {
        sr.sprite = walkingSpriteSheet[(int)((Time.time * 3) % walkingSpriteSheet.Length)];
        sr.flipX = direction < 0;
        var diff = new Vector3(direction, 0, 0);

        var raycast = Physics2D.Raycast(transform.position, Vector2.left, 2, LayerMask.GetMask("Terrain"));
        if (jumpCooldown > 2 && raycast.collider != null)
        {
            jumpCooldown = 0;
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 13, ForceMode2D.Impulse);
        }
        transform.position += diff * Time.deltaTime * 4;
    }
}
