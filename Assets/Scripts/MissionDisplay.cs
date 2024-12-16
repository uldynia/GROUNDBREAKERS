using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MissionDisplay : MonoBehaviour
{
    public Mission mission;
    public TextMeshProUGUI title;
    public Image image;
    public void Init(Mission _mission)
    {
        mission = _mission;
        title.text = mission.title;
        image.sprite = mission.thumbnail;
    }
    public void OnClick() { MissionsManager.instance.Click(mission); }
}
