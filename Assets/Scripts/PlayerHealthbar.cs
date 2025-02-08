using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] GameObject p_healthbar;
    GameObject healthbar;
    Image foreground;
    Entity e;
    TextMeshProUGUI tmpro;
    void Start() {
        StartCoroutine(init());
        IEnumerator init() {
            yield return new WaitForSeconds(1);
            while(PlayerController.instance == null) yield return null;
            healthbar = Instantiate(p_healthbar, GameObject.Find("PlayerHealthbarGroup").transform);
            tmpro = healthbar.GetComponentInChildren<TextMeshProUGUI>();
            tmpro.text = GetComponent<PlayerController>().username;
            foreground = healthbar.transform.Find("background").GetChild(0).GetComponent<Image>();
            e = GetComponent<Entity>();
            e.onDamage += OnDamage;
            if(e.isLocalPlayer) {
                tmpro.text += " (Me)";
            }
            deadSymbol = healthbar.transform.Find("Dead").gameObject;
        }
    }
    GameObject deadSymbol;
    void OnDamage(float damage)
    {
        foreground.fillAmount = Mathf.Clamp(e.health / e.maxHealth, 0, 1);
        deadSymbol.SetActive(e.health < 0);
    }
    ~PlayerHealthbar()
    {
        Destroy(healthbar);
    }
}
