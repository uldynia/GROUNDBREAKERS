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
    }
    // Update is called once per frame
    bool init = false;
    void Update()
    {
        loadoutlist.alpha = selectedSlot == null ? 0 : 1;

        if (init == true || PlayerController.instance == null) return;
        init = true;
        foreach (var tool in PlayerController.instance.GetComponents<PowerTool>())
        {
            if (!tool.selectable) continue;
            powertools.Add(tool.GetType().ToString(), tool);
            var loadoutIcon = Instantiate(p_loadoutIcon, loadoutlist.transform);
            loadoutIcon.GetComponent<Image>().sprite = tool.icon;
            loadoutIcon.GetComponent<UIButton>().onClick.AddListener(() => { SelectTool(tool); });
        }
        SelectSlot(slots[0].button);
        SelectTool(PlayerController.instance.GetComponent<PunchTool>());
        selectedSlot = null;
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
                    selectedToolName.text = slot.powerTool.GetType().ToString();
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
            if (tool != null) if (!tool.selectable) continue;
        }
        selectedSlot.button.transform.GetChild(0).GetComponent<Image>().sprite = tool.icon;
        selectedSlot.powerTool = tool;
        SetTools();
    }

    void SetTools()
    {
        foreach(Transform trf in powerToolsTransform)
        {
            Destroy(trf.gameObject);
        }
        foreach(var slot in slots)
        {
            if (slot.powerTool == null) return;
            var button = Instantiate(p_powerToolButton, powerToolsTransform);
            button.GetComponent<PowerToolButton>().Init(slot.powerTool);
        }
    }
}
