using System.Collections;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CameraObjectMovementComponent : MonoBehaviour
{
    [SerializeField] private Rigidbody manipulatedObject;
    [SerializeField] private float forceMultiplier = 1f;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stoppingOffset = 0.1f;
    private bool _enable = false;
    private bool _isOn = false;
    private Vector3 _forcePoint;

    private void Start()
    {
        //StartCoroutine(MoveObjectToCameraCentre());
    }

    private IEnumerator MoveObjectToCameraCentre()
    {
        Vector3 targetPoint = transform.position + Vector3.forward * distanceFromCamera;
        Ray ray = new Ray(transform.position, (targetPoint - transform.position).normalized);
        RaycastHit hit;
        Collider col = manipulatedObject.GetComponent<Collider>();
        if (Physics.Raycast(ray, out hit, distanceFromCamera * 2f))
        {
            _forcePoint = hit.point;
            Debug.Log(_forcePoint);
        }
        else
        {
            _forcePoint = Vector3.zero;
            Debug.Log("Object not hit with Raycast");
            yield break;
        }

        while (_enable)
        {
            _forcePoint = col.ClosestPoint(targetPoint);
            targetPoint = transform.position + Vector3.forward * distanceFromCamera;
            Vector3 dir = (targetPoint - _forcePoint).normalized;
            if (distanceFromCamera > 0.1f)
            {
                manipulatedObject.AddForceAtPosition(dir * forceMultiplier, _forcePoint);
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