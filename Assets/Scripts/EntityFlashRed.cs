using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(SpriteRenderer))]
public class EntityFlashRed : MonoBehaviour
{
    Color originalColor;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        GetComponent<Entity>().onDamage += (float damage) => sr.color = Color.red;
        
    }

    // Update is called once per frame
    void Update()
    {
        sr.color += (originalColor - sr.color) * Time.deltaTime * 10;
    }
}
