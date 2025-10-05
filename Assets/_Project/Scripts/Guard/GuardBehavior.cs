using UnityEngine;

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
        try
        {
            foreach (var action in actions)
                if (action != null) 
                    await action.Execute(guard);

            OnFinished?.Invoke(this);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
