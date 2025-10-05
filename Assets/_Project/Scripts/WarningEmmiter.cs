using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class WarningEmmiter : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference warningSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Guard>())
        {
            PlayWarning();
        }
    }
    
    private void PlayWarning()
    {
        RuntimeManager.PlayOneShot(warningSound);
    }
}
