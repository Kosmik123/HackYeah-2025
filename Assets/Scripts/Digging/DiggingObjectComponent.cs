using System;
using UnityEngine;
using UnityEngine.Events;

public class DiggingObjectComponent : MonoBehaviour
{
    [SerializeField] private EScale scaleToReduce = EScale.Y;
    [SerializeField] private int objectHP = 10;
    private GameObject _diggableObject;
    private float _baseScale = 0;
    private float _scaleReduceAmount;

    public Action<bool> OnHit;

    public enum EScale
    {
        X,
        Y,
        Z
    }


    private void Start()
    {
        switch (scaleToReduce)
        {
            case EScale.X:
                _baseScale = transform.localScale.x;
                break;
            case EScale.Y:
                _baseScale = transform.localScale.y;
                break;
            case EScale.Z:
                _baseScale = transform.localScale.z;
                break;
        }
        if(_baseScale<0) Debug.LogError($"Diggable object {this} has no valid scale!");

        _scaleReduceAmount = _baseScale/objectHP+1;
        Debug.Log($"Object {this} will reduce scale {scaleToReduce.ToString()} by {_scaleReduceAmount}");
    }

    public void Hit(int dmg)
    {
        ObjectHit(dmg);
    }

    [ContextMenu("Prepare for test")]
    private void Prep()
    {
        switch (scaleToReduce)
        {
            case EScale.X:
                _baseScale = transform.localScale.x;
                break;
            case EScale.Y:
                _baseScale = transform.localScale.y;
                break;
            case EScale.Z:
                _baseScale = transform.localScale.z;
                break;
        }
        if (_baseScale < 0) Debug.LogError($"Diggable object {this} has no valid scale!");

        _scaleReduceAmount = _baseScale / (objectHP + 1);
        Debug.Log($"Object {this} will reduce scale {scaleToReduce.ToString()} by {_scaleReduceAmount}");
    }

    [ContextMenu("Test Me!")]
    private void TestHit()
    {
        Hit(1);
    }

    


    private void ObjectHit(int dmg)
    {
        OnHit?.Invoke(objectHP > 0);
        if (objectHP > 0)
        {
            PlayHitVFX();
            objectHP -= dmg;
            ReduceScale();
        }
        else
        {
            PlayDestroyVFX();
            Despawn();
        }
    }

    private void PlayHitVFX()
    {
        Debug.Log("Will play hit VFX");
    }

    private void Despawn()
    {
        Debug.Log("Will destroy object");
    }

    private void PlayDestroyVFX()
    {
        Debug.Log("Will show VFX of destruction");
    }

    private void ReduceScale()
    {
        switch (scaleToReduce)
        {
            case EScale.X:
                transform.localScale = new Vector3(transform.localScale.x - _scaleReduceAmount, transform.localScale.y, transform.localScale.z) ;
                break;
            case EScale.Y:
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - _scaleReduceAmount, transform.localScale.z);
                break;
            case EScale.Z:
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - _scaleReduceAmount);
                break;
        }
    }

}
