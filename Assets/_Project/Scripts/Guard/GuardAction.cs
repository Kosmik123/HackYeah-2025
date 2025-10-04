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

    public async override UniTask Execute(Guard guard)
    {
        await UniTask.WaitForSeconds(waitTime);
    }
}

[System.Serializable]
public class MoveAction : GuardAction
{
    [SerializeField]
    private SplineContainer spline;

    public async override UniTask Execute(Guard guard)
    {
        await guard.MoveAlongSpline(spline);
    }
}
