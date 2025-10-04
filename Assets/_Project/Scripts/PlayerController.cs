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

    private float _defaultRadius;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        _defaultRadius = controller.radius;
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
        if(verticalVelocity < 0 && controller.isGrounded)
            verticalVelocity = 0f;
        
        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.y = verticalVelocity;
        controller.Move( moveSpeed * Time.deltaTime * move);
        
        //rotate player left right
        float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        
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
        
        if(context.canceled)
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
        
        if(context.canceled)
        {
            lookInput = Vector2.zero;
            // Debug.Log("Look Input Canceled");
        }
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
        isCrouching = crouch;
        controller.height = crouch ? crouchHeight : standHeight;
        controller.radius = crouch ? 0.15f : _defaultRadius;
    }

    private bool IsCeilingAbove()
    {
        Vector3 origin = transform.position;
        float rayLength = 1.6f;
        return Physics.Raycast(origin, Vector3.up, rayLength, ceilingMask);
    }
}
