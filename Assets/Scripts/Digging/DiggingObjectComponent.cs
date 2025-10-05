using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class DiggingObjectComponent : MonoBehaviour
{
    [SerializeField] private EScale scaleToReduce = EScale.Y;
    [SerializeField] private int objectHP = 10;
    [SerializeField] private ParticleSystem VFX;

    private GameObject _diggableObject;
    private float _baseScale = 0;
    private float _scaleReduceAmount;
    private VFXEventAttribute VFXEventAttribute;
    private bool _pendingDestroy;

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

        _scaleReduceAmount = _baseScale/(objectHP+1);
        Debug.Log($"Object {this} will reduce scale {scaleToReduce.ToString()} by {_scaleReduceAmount}");
    }

    private void PlayVFX(RaycastHit hitInfo)
    {
        if (!VFX) return;
        VFX.transform.position = hitInfo.point;
        VFX.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        VFX.Play();
    }

    public void Hit(int dmg, RaycastHit hitInfo)
    {
        PlayVFX(hitInfo);
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
        Hit(1, new RaycastHit());
    }

    


    private void ObjectHit(int dmg)
    {
        OnHit?.Invoke(objectHP > 0);
        if (objectHP > 0)
        {
            objectHP -= dmg;
            ReduceScale();
        }
        else
        {
            PlayDestroyVFX();
            Despawn();
        }
    }


    private void Despawn()
    {
        if (_pendingDestroy) return;
        _pendingDestroy = true;
        Destroy(gameObject);
    }

    private void PlayDestroyVFX()
    {
        // PlayVFX();
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
