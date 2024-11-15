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
    public void SelectSlot(UIButton button)
    {
        foreach(var slot in slots)
        {
            if(slot.button == button)
            {
                selectedSlot = slot;
            }
        }
    }
    void SelectTool(PowerTool tool)
    {
        selectedSlot.powerTool = tool;
        selectedToolName.text = tool.Name;
        selectedToolDescription.text = tool.description;
    }

    // Update is called once per frame
    void Update()
    {
        loadoutlist.alpha = selectedSlot == null ? 0 : 1;
    }
}
