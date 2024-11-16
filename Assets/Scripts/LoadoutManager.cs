using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;
    public GameObject p_loadoutIcon;
    [System.Serializable] public class Slot
    {
        public UIButton button;
        public PowerTool powerTool;
    }
    public Slot[] slots;
    Slot selectedSlot;
    public CanvasGroup loadoutlist;
    PowerTool[] powertools;
    public Transform powerToolsTransform;
    [SerializeField] TextMeshProUGUI selectedToolName, selectedToolDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        powertools = Resources.LoadAll<PowerTool>("PowerTools");
        foreach(var tool in powertools) {
            var loadoutIcon = Instantiate(p_loadoutIcon, loadoutlist.transform);
            loadoutIcon.GetComponent<Image>().sprite = tool.transform.GetChild(0).GetComponent<Image>().sprite;
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
        selectedToolName.text = tool.Name;
        selectedToolDescription.text = tool.description;

        foreach(var slot in slots)
        {
            if (tool == slot.powerTool) return;
        }
        selectedSlot.button.transform.GetChild(0).GetComponent<Image>().sprite = tool.transform.GetChild(0).GetComponent<Image>().sprite;
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
            if (slot.powerTool == null) continue;
            var s = Instantiate(slot.powerTool.gameObject, powerToolsTransform);
            NetworkServer.Spawn(s);

        }
    }
    // Update is called once per frame
    void Update()
    {
        loadoutlist.alpha = selectedSlot == null ? 0 : 1;
    }
}
