using Mirror;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameField;
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
        if (Application.platform == RuntimePlatform.WindowsServer || Application.platform == RuntimePlatform.LinuxServer)
            StartCoroutine(read());
    }
    IEnumerator read()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "server.txt");
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                string text = request.downloadHandler.text;
                if (Transport.active is PortTransport portTransport)
                {
                    ushort port = 0;
                    if (!ushort.TryParse(text, out port))
                    {
                        Debug.Log("Invalid!");
                        Application.Quit();
                    }
                    portTransport.Port = port;
                    NetworkManager.instance.StartServer();
                    Debug.Log($"Server started: {NetworkManagerHUD.ipAddress}, {port}");
                }
            }
            else
            {
                Debug.LogError("Error reading file: " + request.error);
                Application.Quit();
            }
        }

    }
}
