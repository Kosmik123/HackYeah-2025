using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class DiggingObjectComponent : MonoBehaviour
{
    [SerializeField] private EScale scaleToReduce = EScale.Y;
    [SerializeField] private int objectHP = 10;
    [SerializeField] private VisualEffect VFX;
    [SerializeField] private Vector3 VFXvelocityVector;
    [SerializeField] private Color VFXsecondaryColor;
    private GameObject _diggableObject;
    private float _baseScale = 0;
    private float _scaleReduceAmount;
    private VFXEventAttribute VFXEventAttribute;

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
        if (!VFX) Debug.LogError($"No VFX found in {this}");
        else
            VFXEventAttribute = VFX.CreateVFXEventAttribute();
    }

    private void PlayVFX()
    {
        if (!VFX) return;
        // Set event data
        VFXEventAttribute.SetFloat("size", Random.Range(0f, 1f));
        VFXEventAttribute.SetVector3("velocity", VFXvelocityVector);
        // Custom attribute: secondaryColor
        VFXEventAttribute.SetVector3("secondaryColor",
            new Vector3(VFXsecondaryColor.r, VFXsecondaryColor.g, VFXsecondaryColor.b));

        // Data is copied from eventAttribute, so this object can be used again
        VFX.SendEvent("OnPlay", VFXEventAttribute);
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
        Destroy(gameObject);
    }

    private void PlayDestroyVFX()
    {
        PlayVFX();
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
