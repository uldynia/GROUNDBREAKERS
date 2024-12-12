using UnityEngine;

public class PlayerHealthbarGroupHider : MonoBehaviour
{
    [SerializeField] Transform playerHealthbarGroup, button;
    float targetScale = 1;
    void Update()
    {
        playerHealthbarGroup.transform.localScale += (new Vector3(targetScale, 1, 1) - playerHealthbarGroup.transform.localScale) * 10 * Time.deltaTime;
        button.eulerAngles = new Vector3(0, 0, targetScale * 180);
    }
    public void ToggleGroup() {
        targetScale = targetScale == 1 ? 0 : 1;
    }
}
