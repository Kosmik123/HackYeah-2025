using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public List<KeyDialogueSlice> Dialogues;
    
    public async void ShowDialogue(string key)
    {
        try
        {
            var dialogue = Dialogues.Find(d => d.Key == key);
            if (dialogue.Dialogue == null)
            {
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
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    [ContextMenu("Show Test Dialogue")]
    private void ShowTestDialogue()
    {
        ShowDialogue("test");
    }
}
