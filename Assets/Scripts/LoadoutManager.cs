using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : NetworkBehaviour
{
    public static LoadoutManager instance;
    public GameObject p_loadoutIcon, p_powerToolButton;
    [System.Serializable] public class Slot
    {
        public UIButton button;
        public PowerTool powerTool;
    }
    public Slot[] slots;
    Slot selectedSlot;
    public CanvasGroup loadoutlist;
    Dictionary<string, PowerTool> powertools = new();
    public Transform powerToolsTransform;
    [SerializeField] TextMeshProUGUI selectedToolName, selectedToolDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        foreach(var tool in Resources.LoadAll<PowerTool>("PowerTools")) {
            powertools.Add(tool.name, tool);
            var loadoutIcon = Instantiate(p_loadoutIcon, loadoutlist.transform);
            loadoutIcon.GetComponent<Image>().sprite = tool.icon;
            loadoutIcon.GetComponent<UIButton>().onClick.AddListener(() => { SelectTool(tool); });
        }
    }
    public void Close()
    {
        GetComponent<Animator>().Play("close");
        selectedSlot = null;
    }
    public void SelectSlot(UIButton button)
    {
        foreach(var slot in slots)
        {
            if(slot.button == button)
            {
                selectedSlot = slot;
                if(slot.powerTool != null)
                {
                    selectedToolName.text = slot.powerTool.name;
                    selectedToolDescription.text = slot.powerTool.description;
                }
            }
        }
    }
    void SelectTool(PowerTool tool)
    {
        selectedToolName.text = tool.name;
        selectedToolDescription.text = tool.description;

        foreach(var slot in slots)
        {
            if (tool == slot.powerTool) return;
        }
        selectedSlot.button.transform.GetChild(0).GetComponent<Image>().sprite = tool.icon;
        selectedSlot.powerTool = tool;
        SetTools();
    }

    void SetTools()
    {
        foreach(Transform trf in powerToolsTransform)
        {
            NetworkServer.Destroy(trf.gameObject);
        }
        foreach(var slot in slots)
        {
            if (slot.powerTool == null) return;
            CreateSkillObject(slot.powerTool.name);
        }
    }
    [Command(requiresAuthority =false)]
    public void CreateSkillObject(string name, NetworkConnectionToClient client = null)
    {
        var go = Instantiate(powertools[name].gameObject);
        NetworkServer.Spawn(go);
        var identity = go.GetComponent<NetworkIdentity>();
        identity.AssignClientAuthority(client);
        CreateButton(client, identity.netId);
    }
    [TargetRpc]
    public void CreateButton(NetworkConnectionToClient client, uint id)
    {
        var button = Instantiate(p_powerToolButton, powerToolsTransform);
        button.GetComponent<PowerToolButton>().Init(NetworkServer.spawned[id].GetComponent<PowerTool>());
    }
    // Update is called once per frame
    void Update()
    {
        loadoutlist.alpha = selectedSlot == null ? 0 : 1;
    }
}
