using TMPro;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [SerializeField] private TMP_Text tooltip;
    
    public void ShowTooltip(string message)
    {
        tooltip.gameObject.SetActive(true);
        tooltip.text = message;
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
        tooltip.text = "";
    }
    
    [ContextMenu("Show Test Tooltip")]
    private void ShowTestTooltip()
    {
        ShowTooltip("This is a test tooltip message.");
    }
    
    [ContextMenu("Hide Test Tooltip")]
    private void HideTestTooltip()
    {
        HideTooltip();
    }
}
