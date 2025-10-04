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
    [SerializeField] private float miningCooldown = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private string animationName;
    private float _timeFromLastHit = 3f;
    private VFXEventAttribute VFXEventAttribute;

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
        if (CanDig())
        {
            if (playerAnimator)
                playerAnimator.Play(animationName);
            PlayVFX();
            if (!FMODEventReference.IsNull)
                RuntimeManager.PlayOneShotAttached(FMODEventReference, dugObjectComponent.gameObject);
            dugObjectComponent.Hit(damage);
            StartCoroutine(StartCooldown());
            return true;
        }

        return false;
    }

    private IEnumerator StartCooldown()
    {
        _timeFromLastHit = 0f;
        while (_timeFromLastHit <= miningCooldown)
        {
            _timeFromLastHit += Time.deltaTime;
            Debug.Log(_timeFromLastHit);
            yield return null;
        }
    }

    private bool CanDig()
    {
        return Mathf.Abs(_timeFromLastHit - miningCooldown) < 0.1;
    }
}