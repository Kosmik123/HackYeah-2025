using System.Collections;
using FMOD;
using UnityEngine;

public class CameraObjectMovementComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody manipulatedObject;
    [SerializeField] private float forceMultiplier = 1f;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stoppingOffset = 0.1f;
    private bool _enable = false;
    private bool _isOn = false;

    private void Start()
    {
        //StartCoroutine(MoveObjectToCameraCentre());
    }

    private IEnumerator MoveObjectToCameraCentre()
    {
        while (_enable)
        {
            Vector3 targetPoint = transform.position + Vector3.forward * distanceFromCamera;
            Vector3 dir = (targetPoint - manipulatedObject.position).normalized;
            if (distanceFromCamera > 0.1f)
            {
                //TODO: add force at point
                manipulatedObject.AddForce(dir * forceMultiplier);
            }

            if (manipulatedObject.linearVelocity.magnitude > maxSpeed)
                manipulatedObject.linearVelocity = manipulatedObject.linearVelocity.normalized * maxSpeed;
            yield return null;
        }

        yield return null;
    }

    [ContextMenu("ToggleCameraManipulation")]
    private void Toggle()
    {
        if (!_isOn)
        {
            _isOn = true;
            _enable = true;
            StartCoroutine(MoveObjectToCameraCentre());
        }
        else
        {
            _isOn = false;
            _enable = false;
            StopCoroutine(MoveObjectToCameraCentre());
        }
    }
}