using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;

public class MissionsManager : NetworkBehaviour
{
    /* Pre-game*/
    [SerializeField] Transform content;
    public static MissionsManager instance;
    public Dictionary<string, Mission> missions = new();
    public readonly SyncList<string> currentMissions = new();
    public Mission currentMission;
    public GameObject p_missionDisplay;
    void Start() {
        instance=this;
        var missionResource = Resources.LoadAll<Mission>("Missions");
        missions = missionResource.ToDictionary(mission => mission.name);
        if(!isServer) return;
        for (int i = 0; i < 6; i++)
        {
            currentMissions.Add(missionResource[Random.Range(0, missionResource.Count())].name);
        }
    }

    public void Open() {
        GetComponent<Animator>().Play("open");
        foreach(Transform trf in content) Destroy(trf.gameObject);
        foreach(var missionName in currentMissions) {
            var mission = missions[missionName];
            Instantiate(p_missionDisplay, content).GetComponent<MissionDisplay>().Init(mission);
        }
    }
    public void Click(Mission mission) {
        if(!isServer) return;
        headerText = $"Mission selected: {mission.shortName}";
        currentMission = mission;
    }

    /* During game*/
    [SerializeField] TextMeshProUGUI header;
    [SyncVar(hook = "HeaderUpdate")] public string headerText;
    public void HeaderUpdate(string old, string newv) {
        header.text = newv;
    }
}
