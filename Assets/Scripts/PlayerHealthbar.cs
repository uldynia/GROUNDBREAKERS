using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] GameObject p_healthbar;
    GameObject healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthbar = Instantiate(p_healthbar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ~PlayerHealthbar()
    {
        Destroy(healthbar);
    }
}
