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
        Application.targetFrameRate = 120;
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
        if(serverCode.text == "")
        {
            Loading.instance.ShowAlert("Server code cannot be empty.");
            return;
        }
        Loading.instance.SetDoor(false);
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(1f);
            NetworkManager.instance.networkAddress = serverCode.text;
            NetworkManager.instance.StartClient();
            float timeout = 0;
            while (NetworkClient.connectState != ConnectState.Connected)
            {
                timeout += Time.deltaTime;
                yield return null;
                if(timeout > 5)
                {
                    Loading.instance.ShowAlert("Connection time exceeds timeout.\nCheck your internet connection and make sure your server code is correct.");
                    NetworkManager.instance.StopClient();
                    Loading.instance.SetDoor(true);
                    break;
                }
            }
        }
    }
}
