using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class DamageNumberDisplayer : MonoBehaviour
{
    [SerializeField] GameObject p_damageNumber;
    Entity e;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        e = GetComponent<Entity>();
        e.onDamage += OnDamage;
    }

    void OnDamage(float damage)
    {
        StartCoroutine(IDamage(damage));

    }
    IEnumerator IDamage(float damage)
    {
        if (damage < 0) yield break;
        Vector3 random = new Vector3(Random.Range(1f, -1f), Random.Range(0.5f, -0.5f));
        var text = Instantiate(p_damageNumber, transform.position + random, Quaternion.identity);
        text.GetComponentInChildren<TextMeshPro>().text = damage.ToString();

        float duration = 0;
        while ((duration += Time.deltaTime) < 2)
        {
            text.transform.position += random* Time.deltaTime;
            text.transform.localScale += Vector3.one * Time.deltaTime * Mathf.Max(2 - duration * 2, 0);
            yield return null;
        }
        Destroy(text);
    }
}
