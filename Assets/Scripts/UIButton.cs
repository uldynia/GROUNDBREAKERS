using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject p_clickEffect;
    public UnityEvent onClick = new();
    Vector3 targetScale = Vector3.one;
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(click());
        IEnumerator click()
        {
            var clickEffect = Instantiate(p_clickEffect, transform.parent);
            clickEffect.transform.position = transform.position;
            onClick?.Invoke();
            yield return new WaitForSeconds(0.5f);
            Destroy(clickEffect);
        }
    }
    private void Update()
    {
        transform.localScale += Vector3.ClampMagnitude((targetScale - transform.localScale), 0.2f) * Time.deltaTime * 10;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = Vector3.one * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = Vector3.one;
    }
}
