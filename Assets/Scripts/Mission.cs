using Mirror;
using UnityEngine;
[RequireComponent(typeof(NetworkIdentity))]
public abstract class Mission : NetworkBehaviour
{
    public string shortName;
    public string title;
    public Sprite thumbnail;

    
}

public static class MissionReadWriter {
    public static void WriteMission(this NetworkWriter writer, Mission value)
    {
        writer.Write<string>(value.name);
    }

    public static Mission ReadMission(this NetworkReader reader)
    {
        return MissionsManager.instance.missions[reader.Read<string>()];
    }
}