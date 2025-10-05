using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public List<KeyDialogueSlice> Dialogues;

    public bool DialogueStopped { get; private set; } = true;
    
    public async UniTask ShowDialogue(string key)
    {
        try
        {
            if (!DialogueStopped)
            {
                return;
            }
            DialogueStopped = false;
            
            var dialogue = Dialogues.Find(d => d.Key == key);
            if (dialogue.Dialogue == null)
            {
                DialogueStopped = true;
                Debug.LogError($"Dialogue with key {key} not found.");
                return;
            }

            var uiSystem = FindAnyObjectByType<UISystem>();
            
            foreach (var line in dialogue.Dialogue.Slice)
            {
                uiSystem.ShowDialogue(line.Text, line.LetterPause, line.CompleteDelay);
                await UniTask.WaitUntil(() => !uiSystem.DialogueStopped);
                await UniTask.WaitUntil(() => uiSystem.DialogueStopped);
            }

            DialogueStopped = true;
        }
        catch (Exception e)
        {
            DialogueStopped = true;
            Debug.LogError(e);
        }
    }
    
    [ContextMenu("Show Test Dialogue")]
    private async void ShowTestDialogue()
    {
        try
        {
            await ShowDialogue("test");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
