using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpStrength = 5f;
    public float rotationSpeed = 1f;

    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchCenterY = 0f;
    public float standCenterY = 0f;
    public LayerMask ceilingMask;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private CharacterController controller;
    float verticalVelocity = 0f;
    private bool isCrouching = false;
    private bool wantsToStand = false;
    private CameraObjectMovementComponent _cameraObjectMovementComponent;
    private float _defaultRadius;
    private PlayerDiggingComponent _playerDiggingComponent;
    private Animator _playerAnimator;
    private DiggingObjectComponent _lastHitDiggingObjectComponent;
    private CinemachineCamera _cinemachineCamera;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        _defaultRadius = controller.radius;

        _cameraObjectMovementComponent = Camera.main.GetComponent<CameraObjectMovementComponent>();
        if (!_cameraObjectMovementComponent) Debug.LogError("CameraComponent not found on main camera!");
        _playerDiggingComponent = GetComponent<PlayerDiggingComponent>();
        if (!_playerDiggingComponent) Debug.LogError("No player digging component found on player!");
        _playerAnimator = GetComponentInChildren<Animator>();
        if (!_playerAnimator) Debug.LogError("No animator on player!");
        _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        if(!_cinemachineCamera)Debug.LogError("Cinemachine component not found on player controller!");
    }

    void OnEnable()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        //Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (verticalVelocity < 0 && controller.isGrounded)
            verticalVelocity = 0f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.y = verticalVelocity;
        controller.Move(moveSpeed * Time.deltaTime * move);

        //rotate player left right
        float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        var currentSpeed = controller.velocity.magnitude;
        _playerAnimator.SetFloat("Speed", currentSpeed);
        // Try to stand up if crouching and not holding crouch
        if (isCrouching && wantsToStand && !IsCeilingAbove())
        {
            SetCrouch(false);
            wantsToStand = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            Debug.Log("Move Input: " + moveInput);
        }

        if (context.canceled)
        {
            moveInput = Vector2.zero;
            Debug.Log("Move Input Canceled");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Try Jumped");

        if (context.performed && controller.isGrounded)
        {
            verticalVelocity = jumpStrength; // Jump strength
            Debug.Log("Jumped");
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            lookInput = Vector2.zero;
            // Debug.Log("Look Input Canceled");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var distance = _cameraObjectMovementComponent.GetMaxDistance();
            Ray ray = new Ray(_cameraObjectMovementComponent.transform.position, Camera.main.transform.forward);
            RaycastHit[] hits =
                Physics.RaycastAll(ray, distance, ceilingMask);
            if (hits.Length == 0) return;
            foreach (RaycastHit hit in hits)
            {
                _lastHitDiggingObjectComponent = hit.transform.gameObject.GetComponent<DiggingObjectComponent>();
                if (_lastHitDiggingObjectComponent)
                {
                    var interactable = hit.transform.gameObject.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                        break;
                    }

                    var dug = _playerDiggingComponent.CanDigObject();
                    if (dug)
                    {
                        _playerAnimator.SetTrigger("Dig");
                    }

                    break;
                }

                if (hit.transform.gameObject.GetComponent<Rigidbody>())
                {
                    _cameraObjectMovementComponent.MoveObject(hit.transform.gameObject);
                    break;
                }
            }
        }

        if (context.canceled)
        {
            _cameraObjectMovementComponent.ReleaseObject();
        }
    }

    public void DiggingAnimationEnded()
    {
        _playerDiggingComponent.TryDigObject(_lastHitDiggingObjectComponent);
    }


    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log("Crouch Input: " + context.phase);
        if (context.performed)
        {
            SetCrouch(true);
            wantsToStand = false;
        }

        if (context.canceled)
        {
            wantsToStand = true;
        }
    }
    
    private void SetCrouch(bool crouch)
    {
        if (crouch)
            _playerAnimator.gameObject.transform.localPosition = new Vector3(
                _playerAnimator.gameObject.transform.localPosition.x,
                _playerAnimator.gameObject.transform.localPosition.y + 0.55f,
                _playerAnimator.gameObject.transform.localPosition.z);
        else
            _playerAnimator.gameObject.transform.localPosition = new Vector3(
                _playerAnimator.gameObject.transform.localPosition.x,
                _playerAnimator.gameObject.transform.localPosition.y - 0.55f,
                _playerAnimator.gameObject.transform.localPosition.z);
        if (crouch)
            _cinemachineCamera.gameObject.transform.localPosition = new Vector3(0.23f, 0.6f, 0.4f);
        else
            _cinemachineCamera.gameObject.transform.localPosition = new Vector3(0, 0.815f, 0.072f);

        isCrouching = crouch;
        controller.height = crouch ? crouchHeight : standHeight;
        controller.radius = crouch ? 0.5f : _defaultRadius;
        _playerAnimator.SetBool("Crouch", crouch);
    }

    private bool IsCeilingAbove()
    {
        Vector3 origin = transform.position;
        float rayLength = 1.6f;
        return Physics.Raycast(origin, Vector3.up, rayLength, ceilingMask);
    }
}