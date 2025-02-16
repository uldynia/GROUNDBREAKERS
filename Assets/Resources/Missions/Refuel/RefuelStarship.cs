using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RefuelStarship : Mission
{
    public static RefuelStarship instance;
    public int progress;
    public HoldButton button;
    public override void StartMission()
    {
        base.StartMission();
        instance = this;
        MissionsManager.instance.headerText = "Head to the Starship.";
        transform.position = Physics2D.Raycast(new Vector2(Takama.instance.points[2].x, Takama.instance.points[2].y), Vector2.down).point + Vector2.up * 10f;
    }
    public bool found;
    [Server]
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!found)
        {
            found = true;
            MissionsManager.instance.headerText = "Deposit fuel into the ship.";
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == PlayerController.instance.gameObject)
            transform.GetChild(0).gameObject.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == PlayerController.instance.gameObject)
            transform.GetChild(0).gameObject.SetActive(false);
    }
    float cooldown = 0;
    void Update()
    {
        if ((cooldown += Time.deltaTime) < 0.1f) return;
        if (button.isHolding)
        {
            Deposit();
            cooldown = 0;
        }
    }
    [Command(requiresAuthority = false)]
    public void Deposit(NetworkConnectionToClient sender = null)
    {
        if (progress >= 150) return;
        if (sender == null) return;
        HashSet<NetworkIdentity> tmp = new HashSet<NetworkIdentity>(sender.owned);
        Inventory inv;
        foreach (NetworkIdentity netIdentity in tmp)
        {
            if (netIdentity.TryGetComponent<Inventory>(out inv))
            {
                if (inv.RemoveItem(InventoryManager.instance.items["Fuel"], 1)
                    #if UNITY_EDITOR
                    || true
                    #endif
                    )
                {
                    progress += 1;
                    MissionsManager.instance.headerText = $"Current progress: {progress} / 150";
                    if (progress >= 150) StartCoroutine(MissionEnd());
                }
                return;
            }
        }
    }
    IEnumerator MissionEnd()
    {
        int missionEndTimer = 10;
        MissionsManager.instance.headerText = "Mission complete!\n<size=10>The Starship will leave in 10 seconds. Get to the ship!</size>";
        yield return new WaitForSeconds(5);
        while(missionEndTimer > 0)
        {
            MissionsManager.instance.headerText = $"Get to the ship!\n<size=10>{missionEndTimer--} seconds remaining.</size>";
            yield return new WaitForSeconds(1
                #if UNITY_EDITOR
                -0.8f
                #endif
                );
        }
        float position = transform.position.y + 50;
        var pos = Vector3Int.RoundToInt(transform.position);
        Takama.instance.LineFill(0, pos, pos + Vector3Int.up * 200, 30);
        float i = 1;
        while (transform.position.y < position)
        {
            transform.position += Vector3.up * Time.deltaTime * (i += Time.deltaTime);
            
            yield return null;
        }
        Takama.instance.SetWon(true);
        Takama.instance.EndGame();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 400, 100, 20), "Teleport to objective")) PlayerController.instance.transform.position = transform.position;
    }
#endif
}