using System.Collections;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CameraObjectMovementComponent : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 3f;
    [SerializeField] private float distanceFromCamera = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float stoppingOffset = 0.1f;
    [SerializeField] private float distanceToRelease = 3f;
    private Rigidbody _manipulatedObject;
    private bool _enable = false;
    private Vector3 _forcePoint;

    private void Start()
    {
        //StartCoroutine(MoveObjectToCameraCentre());
    }

    public void MoveObject(GameObject objectToMove)
    {
        var rb = objectToMove.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError($"RigidBody not found on {objectToMove}");
            return;
        }
        _manipulatedObject = rb;
        Toggle(true);
    }

    public void ReleaseObject()
    {
        Toggle(false);
    }

    private IEnumerator MoveObjectToCameraCentre()
    {
        Vector3 targetPoint = transform.position + transform.forward * distanceFromCamera;
        Ray ray = new Ray(transform.position, (targetPoint - transform.position).normalized);
        RaycastHit hit;
        Collider col = _manipulatedObject.GetComponent<Collider>();
        if (Physics.Raycast(ray, out hit, distanceFromCamera))
        {
            _forcePoint = hit.point;
            Debug.Log("object grabbed");
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
            targetPoint = transform.position + transform.forward * distanceFromCamera;
            Vector3 dir = (targetPoint - _forcePoint).normalized;
            Vector3 toTarget = targetPoint - _manipulatedObject.position;
            float currentDistance = toTarget.magnitude;
            if (currentDistance > distanceToRelease)
            {
                Toggle(false);
                yield break;
            }
            if (currentDistance > stoppingOffset)
            {
                _manipulatedObject.AddForceAtPosition(dir * forceMultiplier, _forcePoint);
            }

            if (_manipulatedObject.linearVelocity.magnitude > maxSpeed)
                _manipulatedObject.linearVelocity = _manipulatedObject.linearVelocity.normalized * maxSpeed;
            yield return null;
        }

        yield return null;
    }

    [ContextMenu("ToggleCameraManipulation")]
    private void Toggle(bool enable)
    {
        if (enable)
        {
            if (_enable) return;
            _enable = true;
            StartCoroutine(MoveObjectToCameraCentre());
        }
        else
        {
            _enable = false;
            Debug.Log("object released");
            StopCoroutine(MoveObjectToCameraCentre());
        }
    }

    public float GetMaxDistance()
    {
        return distanceFromCamera;
    }
}