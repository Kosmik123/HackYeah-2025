using System;
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
    
    private Vector3 lastPosition;

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
}
