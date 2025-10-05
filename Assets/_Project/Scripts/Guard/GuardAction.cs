using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public abstract class GuardAction
{
    public event System.Action<GuardAction> OnFinished;
    public abstract UniTask Execute(Guard guard);
    protected void Finished() => OnFinished?.Invoke(this);
}

[System.Serializable]
public class WaitAction : GuardAction
{
    [SerializeField]
    private float waitTime;

    public override async UniTask Execute(Guard guard)
    {
        await UniTask.WaitForSeconds(waitTime);
    }
}

[System.Serializable]
public class MoveAction : GuardAction
{
    [SerializeField]
    private SplineContainer spline;

    public override async UniTask Execute(Guard guard)
    {
        await guard.MoveAlongSpline(spline);
    }
}

[System.Serializable]
public class StartDialogueAction : GuardAction
{
    [SerializeField]
    private DialogueSystem dialogueSystem;
    [SerializeField]
    private string dialogueKey;

    public override async UniTask Execute(Guard guard)
    {
        if (dialogueSystem != null)
        {
            await dialogueSystem.ShowDialogue(dialogueKey);
        }
    }
}
