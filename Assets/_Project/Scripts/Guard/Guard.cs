using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class Guard : MonoBehaviour
{
    private static readonly int Speed1 = Animator.StringToHash("Speed");

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private StudioEventEmitter footstepEmitter;
    [SerializeField] private StudioEventEmitter auraEmitter;
    [SerializeField] private SplineAnimate splineAnimate;

    public float Speed { get; private set; }
    public Action<Guard> OnPathEnded { get; set; }
    public Action<Guard> OnPathStarted { get; set; }
    
    private Vector3 lastPosition;

    public async UniTask MoveAlongSpline(SplineContainer spline)
    {
        splineAnimate.Container = spline;
        splineAnimate.Restart(true);
        OnPathStarted?.Invoke(this);
        await UniTask.WaitWhile(() => splineAnimate.IsPlaying);
    }

    private void Update()
    {
        UpdateSpeed();
        UpdateAnimator();
    }

    public void EmitFootstep()
    {
        footstepEmitter.Stop();
        footstepEmitter.Play();
    }

    private void UpdateSpeed()
    {
        Speed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        
        lastPosition = transform.position;
        Debug.Log(Speed);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(Speed1, Speed);
    }
}
