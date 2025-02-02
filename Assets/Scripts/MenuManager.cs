using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameField, serverCode;
    [SerializeField] CanvasGroup menu;
    [SerializeField] Image lobby;
    void Start()
    {
        if(PlayerPrefs.HasKey("username")) {
            usernameField.text = PlayerPrefs.GetString("username");
            PlayerController.menuUsername = usernameField.text;
        }
        usernameField.onValueChanged.AddListener((string _name) =>
        {
            PlayerController.menuUsername = _name;
            PlayerPrefs.SetString("username", _name);
        });
    }
    public void GoToLobby()
    {
        StartCoroutine(FadeOut());
        IEnumerator FadeOut()
        {
            while( (menu.alpha -= Time.deltaTime * 5) > 0)
            {
                #if !UNITY_EDITOR
                yield return null;
                #endif
            }
            #if !UNITY_EDITOR
            yield return new WaitForSeconds(0.5f);
            #endif
            float i = 1;
            while ((lobby.fillAmount += Time.deltaTime * (i += Time.deltaTime * 5)) < 1)
            {
                #if !UNITY_EDITOR
                yield return null;
                #endif
            }
            lobby.raycastTarget = true;
            yield return null;
        }
    }
    public void CreateGame()
    {
        StartCoroutine(wait());
        IEnumerator wait()
        {
            #if !UNITY_EDITOR
                Loading.instance.SetDoor(false);
                yield return new WaitForSeconds(3f);
            #endif
            NetworkManager.instance.StartHost();
            yield return null;
        }
    }
    public void JoinGame()
    {
        Loading.instance.SetDoor(false);
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(3f);
            NetworkManager.instance.networkAddress = serverCode.text;
            NetworkManager.instance.StartClient();
        }
    }
}
