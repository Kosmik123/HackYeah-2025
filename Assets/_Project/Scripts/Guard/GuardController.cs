using HackYeah2025;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeField]
    private Guard guard;

    [SerializeField]
    private GuardBehavior[] behaviors;

    [SerializeField]
    private int currentActionIndex = -1;
    private int previousActionIndex = -1;

    private void OnEnable()
    {
        WorldTime.OnTimeChanged += DecideAction;
    }

    private void OnDisable()
    {
        WorldTime.OnTimeChanged -= DecideAction;
    }

    public void ActivateAction(int index)
    {
        WorldTime.OnTimeChanged -= DecideAction;

        currentActionIndex = index;
        behaviors[currentActionIndex].OnFinished += GuardController_OnFinished;
        behaviors[currentActionIndex].StartEvent(guard);
    }

    private void DecideAction()
    {
        for (int i = behaviors.Length - 1; i >= 0; i--)
        {
            if (i == previousActionIndex)
                continue;

            if (behaviors[i].AreConditionsMet())
            {
                ActivateAction(i);
                break;
            }
        }
    }

    private void GuardController_OnFinished(GuardBehavior finishedEvent)
    {
        finishedEvent.OnFinished -= GuardController_OnFinished;
        previousActionIndex = currentActionIndex;
        currentActionIndex = -1;
        WorldTime.OnTimeChanged += DecideAction;
    }
}
