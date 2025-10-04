using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using TMPro;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text tooltip;
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private StudioEventEmitter dialogueSoundEmitter;

    public bool DialogueStopped { get; set; } = true;
    
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
    
    public async void ShowDialogue(string text, float letterPause = 0.15f, float completeDelay = 1.0f)
    {
        try
        {
            DialogueStopped = false;
            dialogue.gameObject.SetActive(true);
            dialogue.text = "";
        
            while (text.Length > 0)
            {
                dialogue.text += text[0];
                text = text.Remove(0, 1);
                dialogueSoundEmitter.Play();
                await UniTask.WaitForSeconds(letterPause);
            }
            await UniTask.WaitForSeconds(completeDelay);
        
            dialogue.gameObject.SetActive(false);
            DialogueStopped = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    [ContextMenu("Show Test Dialogue")]
    private void ShowTestDialogue()
    {
        ShowDialogue("This is a test dialogue message.");
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
