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
    
    [Header("Settings")]
    [SerializeField] private float stayDuration = 3f;

    public float Speed { get; private set; }
    public Action OnPathEnded { get; set; }
    public Action OnPathStarted { get; set; }
    
    private Vector3 lastPosition;

    private void Awake()
    {
        splineAnimate.Completed += OnPathCompleted;
    }

    private void OnDestroy()
    {
        splineAnimate.Completed -= OnPathCompleted;
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
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(Speed1, Speed);
    }
    
    private async void OnPathCompleted()
    {
        try
        {
            OnPathEnded?.Invoke();

            await UniTask.WaitForSeconds(stayDuration);
        
            splineAnimate.Restart(true);
            OnPathStarted?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
