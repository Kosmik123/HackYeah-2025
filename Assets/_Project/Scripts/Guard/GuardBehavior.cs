using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]   
public class GuardBehavior
{
    public event System.Action<GuardBehavior> OnFinished;

    [SerializeReference, SubclassSelector]
    private GuardEventCondition[] conditions;

    [SerializeReference, SubclassSelector]
    private GuardAction[] actions;

    public bool AreConditionsMet()
    {
        if (conditions.Length <= 0)
            return false;

        foreach (var condition in conditions)
            if (condition.IsMet() == false)
                return false;

        return true;
    }

    public async void StartEvent(Guard guard)
    {
        foreach (var action in actions)
            await action.Execute(guard);
        guard.OnPathEnded += Guard_OnPathEnded;
    }


    private void Guard_OnPathEnded(Guard guard)
    {
        guard.OnPathEnded -= Guard_OnPathEnded;
        OnFinished?.Invoke(this);
    }
}


