using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PlayerDiggingComponent : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private VisualEffect VFX;
    [SerializeField] private EventReference FMODEventReference;
    [SerializeField] private Vector3 VFXvelocityVector;
    [SerializeField] private Color VFXsecondaryColor;
    [SerializeField] private int damage = 1;
    [SerializeField] private string animationName;
    private VFXEventAttribute VFXEventAttribute;
    private bool _canDig = true;

    private void Start()
    {
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

    public bool TryDigObject(DiggingObjectComponent dugObjectComponent)
    {
        if (_canDig)
        {
            if (playerAnimator)
                playerAnimator.Play(animationName);
            PlayVFX();
            if (!FMODEventReference.IsNull)
                RuntimeManager.PlayOneShotAttached(FMODEventReference, dugObjectComponent.gameObject);
            dugObjectComponent.Hit(damage);
            _canDig = false;
            return true;
        }

        return false;
    }

    public bool CanDigObject()
    {
        return _canDig;
    }

    public void AnimationEnded()
    {
        _canDig = true;
    }


}