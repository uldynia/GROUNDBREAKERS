using System.Collections.Generic;
using Mirror;
using UnityEngine;

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
        if (sender == null) return;
        HashSet<NetworkIdentity> tmp = new HashSet<NetworkIdentity>(sender.owned);
        Inventory inv;
        foreach (NetworkIdentity netIdentity in tmp)
        {
            if (netIdentity.TryGetComponent<Inventory>(out inv))
            {
                if (inv.RemoveItem(InventoryManager.instance.items["Fuel"], 1))
                {
                    progress += 1;
                    MissionsManager.instance.headerText = $"Current progress: {progress} / 150";
                }
                return;
            }
        }
    }
}
