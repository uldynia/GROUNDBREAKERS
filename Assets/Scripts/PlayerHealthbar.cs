using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] GameObject p_healthbar;
    GameObject healthbar;
    Image foreground;
    Entity e;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthbar = Instantiate(p_healthbar, GameObject.Find("PlayerHealthbarGroup").transform);
        healthbar.GetComponentInChildren<TextMeshProUGUI>().text = PlayerController.instance.username;
        foreground = healthbar.transform.Find("background").GetChild(0).GetComponent<Image>();
        e = GetComponent<Entity>();
        e.onDamage += OnDamage;
    }

    void OnDamage(float damage)
    {
        foreground.fillAmount = Mathf.Clamp(e.health / e.maxHealth, 0, 1);
    }
    ~PlayerHealthbar()
    {
        Destroy(healthbar);
    }
}
