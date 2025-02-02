using LightReflectiveMirror;
using UnityEngine;
using Mirror;
public class ConnectionManager : MonoBehaviour
{
    LightReflectiveMirrorTransport transport;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transport = (LightReflectiveMirrorTransport)NetworkManager.instance.transport;
        transport.disconnectedFromRelay.AddListener( OnDisconnect);
    }

    public void OnDisconnect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
