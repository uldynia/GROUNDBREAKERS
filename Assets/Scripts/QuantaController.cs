using UnityEngine;
using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
public class QuantaController : NetworkBehaviour
{
    [SerializeField] Sprite[] walkingSpriteSheet;
    SpriteRenderer sr;
    [SerializeField] float damage = 5;
    [SerializeField] GameObject p_bullet;
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
        PATROL
    }
    public STATE state;
    [SyncVar] float stateDuration;
    [SyncVar] Vector3 direction;
    void Update()
    {
        if (!isServer) return;
        stateDuration += Time.deltaTime;

        switch (state)
        {
            case STATE.IDLE:
                sr.sprite = walkingSpriteSheet[0];
                if (stateDuration > 1)
                {
                    state = STATE.PATROL;
                    stateDuration = 0;
                }
                break;
            case STATE.PATROL:
                Route();
                if (stateDuration > 1.5f)
                {
                    state = STATE.IDLE;
                    stateDuration = 0;
                    if(players.Count > 0)
                    {
                        direction = (players[0].transform.position - transform.position).normalized + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                        var bullet = Instantiate(p_bullet, transform.position, Quaternion.identity).GetComponent<QuantaBullet>();
                        NetworkServer.Spawn(bullet.gameObject);
                        bullet.init(direction, damage);
                    }
                    else
                        direction = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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
            state = STATE.PATROL;
            stateDuration = 0;
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
        sr.flipX = state != STATE.IDLE;
        transform.position += direction * Time.deltaTime * 4;
    }
}
