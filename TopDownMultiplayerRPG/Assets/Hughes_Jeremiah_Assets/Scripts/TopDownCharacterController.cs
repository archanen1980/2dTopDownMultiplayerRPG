using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f; // Normal movement speed
    public float sprintSpeedMultiplier = 2f; // Multiplier for sprint speed
    [SerializeField] private bool canSprint = true; // Toggle for sprinting
    private float currentMoveSpeed;

    [Header("Rotation")]
    public float rotationSpeed = 500f; // Speed of rotation

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool useAnimator = true; // Toggle for using the animator
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    private Vector2 moveInput; // Stores input values
    private float currentAngle = 0f; // Stores current rotation angle
    private bool isSprinting = false; //Track sprinting state
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (useAnimator)
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogError("Animator not found, but useAnimator is enabled. Disabling useAnimator.");
                    useAnimator = false;
                }
            }
        }

        currentMoveSpeed = moveSpeed;

        //Create Player Input Actions
        playerInputActions = new PlayerInputActions();

        //Enable Map
        OnEnable();
    }

    private void OnEnable()
    {
        //Enable Player Input Actions
        playerInputActions.Player.Enable();
        //Subscribes to movement event
        playerInputActions.Player.Movement.performed += OnMovementPerformed;
        playerInputActions.Player.Movement.canceled += OnMovementCancelled;
        //Subscribe to Sprint Event
        playerInputActions.Player.Sprint.performed += OnSprintPerformed;
        playerInputActions.Player.Sprint.canceled += OnSprintCancelled;
    }
    private void OnDestroy()
    {
        //Unsubscribe from events to prevent memory leaks
        playerInputActions.Player.Movement.performed -= OnMovementPerformed;
        playerInputActions.Player.Movement.canceled -= OnMovementCancelled;
        playerInputActions.Player.Sprint.performed -= OnSprintPerformed;
        playerInputActions.Player.Sprint.canceled -= OnSprintCancelled;
    }

    private void FixedUpdate()
    {
        HandleSprinting();
        if(moveInput != Vector2.zero){
           HandleMovement();
        }
    }

    private void HandleMovement()
    {
        // Check if there is movement input
        if (moveInput != Vector2.zero)
        {
            // Calculate target rotation angle (atan2 returns radians, so we convert to degrees)
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;

            // Smoothly rotate towards the target angle
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

            // Move the player **in the direction of the keypress**
            transform.position += (Vector3)moveInput * currentMoveSpeed * Time.deltaTime;
        }
    }

    private void HandleSprinting()
    {
        if (canSprint)
        {
            if (isSprinting)
            {
                currentMoveSpeed = moveSpeed * sprintSpeedMultiplier;
            }
            else
            {
                currentMoveSpeed = moveSpeed;
            }
        }
        else
        {
            currentMoveSpeed = moveSpeed;
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            if (moveInput == Vector2.zero)
            {
                animator.SetBool(IsMoving, false);
            }
            else
            {
                animator.SetBool(IsMoving, true);
                animator.SetFloat(MoveX, moveInput.x);
                animator.SetFloat(MoveY, moveInput.y);
            }
        }
    }

    //Input System Methods
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        HandleMovement();
    }
    private void OnMovementCancelled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        HandleMovement();
    }
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = true;
        HandleSprinting();
    }
    private void OnSprintCancelled(InputAction.CallbackContext context)
    {
        isSprinting = false;
        HandleSprinting();
    }
}
