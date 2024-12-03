using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] GameObject p_healthbar;
    GameObject healthbar;
    Image foreground;
    Entity e;
    bool init = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (init || PlayerController.instance == null) return;
        init = true;
        healthbar = Instantiate(p_healthbar, GameObject.Find("PlayerHealthbarGroup").transform);
        var tmpro = healthbar.GetComponentInChildren<TextMeshProUGUI>();
        tmpro.text = PlayerController.instance.username;
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
