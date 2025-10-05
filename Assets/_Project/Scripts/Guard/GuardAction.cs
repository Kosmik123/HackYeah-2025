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

    GuardDetection detection;
    
    public override async UniTask Execute(Guard guard)
    {
        detection = guard.GetComponent<GuardDetection>();
        detection.StartLookingForPlayer();
        await UniTask.WaitForSeconds(waitTime);
        detection.StopLookingForPlayer();
        // Finished();
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

[System.Serializable]
public class ShowScenarioObjects : GuardAction
{
    [SerializeField] private OutlineScenarioManager scenarioManager;

    public override UniTask Execute(Guard guard)
    {
        scenarioManager.ActivateScenario(ScenarioType.Room);
        
        return UniTask.CompletedTask;
    }
}
