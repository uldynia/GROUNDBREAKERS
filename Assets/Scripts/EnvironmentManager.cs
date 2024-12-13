using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnvironmentManager : NetworkBehaviour
{
    [SyncVar(hook = "ChangeBackground")] public Item background;
    [SyncVar] public float lightLevel = 1;
    [SerializeField] Light2D globalLight;
    SpriteRenderer sr;
    public static EnvironmentManager instance;
    void Start() {
        instance=this;
        sr = GetComponent<SpriteRenderer>();
    }
    void ChangeBackground(Item oldBackground, Item newBackground) {
        sr.sprite = newBackground.sprite;
    }
    void Update()
    {
        globalLight.intensity = lightLevel;
        transform.position += (new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y) - transform.position * 1.01f) * Time.deltaTime * 100;
    }
}
