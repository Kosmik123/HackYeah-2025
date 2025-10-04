using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpStrength = 5f;
    public float rotationSpeed = 1f;
    
    private Vector2 moveInput;
    private Vector2 lookInput;
    private CharacterController controller;
    float verticalVelocity = 0f;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.y = verticalVelocity;
        controller.Move( moveSpeed * Time.deltaTime * move);
        
        if (controller.isGrounded)
            verticalVelocity = 0f;
        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        
        //rotate player left right
        float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
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
}
