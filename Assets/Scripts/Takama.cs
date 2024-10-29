using UnityEngine;

public class Takama : MonoBehaviour // the main game. Izumo is the lobby.
{
    public static Takama instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
