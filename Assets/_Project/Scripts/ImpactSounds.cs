using FMODUnity;
using UnityEngine;

public class ImpactSounds : MonoBehaviour
{
    [SerializeField] private EventReference ImpactSound;
    [SerializeField] private float minVelocity = 1f;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float cooldown = 0.8f;
    private float elapsedTime = 0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (elapsedTime < cooldown) return;
        elapsedTime = 0f;
        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < minVelocity) return;
        float volume = Mathf.InverseLerp(minVelocity, maxVelocity, velocity);
        RuntimeManager.PlayOneShot(ImpactSound, collision.contacts[0].point);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }
}
