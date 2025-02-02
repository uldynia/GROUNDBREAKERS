using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Loading : MonoBehaviour
{
    public static Loading instance; 
    [SerializeField] Image left, right; 
    [SerializeField] TextMeshProUGUI text; 

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this; 
    }
    public void SetDoor(bool isOpen)
    {
        float targetPosition = isOpen ? 700 : 0; 
        StartCoroutine(LerpRoutine(targetPosition, 2)); 
        if(!isOpen)
            StartCoroutine(LoadingText());
    }

    private IEnumerator LerpRoutine(float targetPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float newX = Mathf.Lerp(right.transform.localPosition.x, targetPosition, t);
            right.transform.localPosition = new Vector3(newX, 0, 0);
            left.transform.localPosition = new Vector3(-newX, 0, 0);

            elapsedTime += Time.deltaTime;

            yield return null; 
        }
        text.text = "";
        right.transform.localPosition = new Vector3(targetPosition, 0, 0);
        left.transform.localPosition = new Vector3(-targetPosition, 0, 0);
    }
    private IEnumerator LoadingText()
    {
        yield return new WaitForSeconds(0.5f);
        string[] loadingMessages = { "Loading", "Loading.", "Loading..", "Loading..." };
        int index = 0;

        while (index < 7) 
        {
            text.text = loadingMessages[(index += 1) % loadingMessages.Length];
            yield return new WaitForSeconds(0.5f); 
        }
        text.text = "";
    }
    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] CanvasGroup alert;
    public void ShowAlert(string text)
    {
        alert.gameObject.SetActive(true);
        alertText.text = text;
        StartCoroutine(show());
        IEnumerator show()
        {
            alert.alpha = 0;
            while ((alert.alpha += Time.deltaTime * 5) < 1) yield return null;
        }
    }
    public void HideAlert()
    {
        StartCoroutine(Hide());
        IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.5f);
            alert.alpha = 1;
            while ((alert.alpha -= Time.deltaTime * 5)> 0) yield return null;
            alert.gameObject.SetActive(false);
        }
    }
}
