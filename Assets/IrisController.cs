using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

public class IrisController : Mission
{
    [SerializeField] GameObject p_bullet;
    [SerializeField] GameObject[] hammers;
    [SerializeField] BoxCollider2D trigger;
    bool active = false;
    [SerializeField] float bulletDamage = 5;
    float direction = 0;
    Entity e;
    public enum STATE
    {
        SHOWCUTSCENE,
        FIREBULLET,
        //USEHAMMER,
        CHARGE,
        SLAM // make sure SLAM is last in the enum
    }
    STATE state;
    float stateDuration = 0;
    Vector3 origin;
    public override void StartMission()
    {
        base.StartMission();
        transform.position = origin = Physics2D.Raycast(new Vector2(Takama.instance.points[2].x, Takama.instance.points[2].y), Vector2.down).point + Vector2.up * 5f;
        MissionsManager.instance.headerText = "Find the boss.";
        e = GetComponent<Entity>();
        e.onDeath += OnDeath;
    }
    void Update()
    {
        if (!isServer || !active) return;
        stateDuration += Time.deltaTime;
        MissionsManager.instance.headerText = $"Defeat the boss.\n<color=red>{new string('█', Mathf.RoundToInt(10 * e.health / e.maxHealth)) }</color>";
        switch (state)
        {
            case STATE.SHOWCUTSCENE:
                if(stateDuration > 4)
                {
                    Destroy(trigger);
                    state = STATE.FIREBULLET;
                    stateDuration = 0;
                }
                break;
            case STATE.FIREBULLET:
                if(stateDuration == Time.deltaTime)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var bullet = Instantiate(p_bullet, transform.position, Quaternion.identity).GetComponent<QuantaBullet>();
                        NetworkServer.Spawn(bullet.gameObject);
                        bullet.init( (GetLockedOn().transform.position - transform.position).normalized * i * 0.5f + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), bulletDamage);
                    }
                }
                if(stateDuration > 5)
                {
                    SetRandomState();
                }
                break;
            case STATE.CHARGE:
                if (stateDuration == Time.deltaTime)
                {
                    var lockedOn = GetLockedOn();
                    transform.position = lockedOn.transform.position + Vector3.left * 10;
                }
                if(stateDuration > 1 && stateDuration < 2)
                {
                    transform.position += Vector3.right * 30 * Time.deltaTime;
                }
                if (stateDuration > 5)
                {
                    transform.position = origin;
                    SetRandomState();
                }

                break;
            case STATE.SLAM:
                if (stateDuration == Time.deltaTime)
                {
                    var lockedOn = GetLockedOn();
                    transform.position = lockedOn.transform.position + Vector3.up * 15;
                }
                if (stateDuration > 1 && stateDuration < 2)
                {
                    transform.position += Vector3.down * 10 * Time.deltaTime;
                }
                if (stateDuration > 2 && stateDuration < 4) transform.position += Vector3.up * 5 * Time.deltaTime;
                if (stateDuration > 5)
                {
                    transform.position = origin;
                    SetRandomState();
                }

                break;
        }
    }
    void OnDeath(float hp)
    {
        active = false;
        MissionsManager.instance.headerText = "The boss has been defeated!";
        // play death animation
        StartCoroutine(death());
        IEnumerator death()
        {
            Takama.instance.SetWon(true);
            var sr = GetComponent<SpriteRenderer>();
            var color = 1f;
            while((color -= (Time.deltaTime * 0.2f)) > 0)
            {
                sr.color = Color.white * color;
                yield return null;
            }
            Takama.instance.EndGame();
        }
    }
    public PlayerController GetLockedOn()
    {
        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
        if (players.Count == 0) return players[0];
        foreach(var player in players)
        {
            if(Vector3.Distance(transform.position, player.transform.position) > 20) players.Remove(player);
        }
        return players[Random.Range(0, players.Count)];
    }
    public void SetRandomState()
    {
        stateDuration = 0;
        state = (STATE)(Random.Range(1, (int)STATE.SLAM));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isServer) return;
        if(collision.CompareTag("Player") && e.health > 0)
        {
            active = true;
        }
    }
    #if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 400, 100, 20), "Teleport to objective")) PlayerController.instance.transform.position = transform.position;
        GUI.Box(new Rect(100, 300, 150, 20), $"STATE: {state}");
    }
    #endif
}
