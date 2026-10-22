using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController3D : MonoBehaviour, IDataPersistence
{
    [SerializeField] private float WalkSpeed = 5.5f;
    [SerializeField] private float Gravity = -20f;

    private float verticalVelocity;

    [SerializeField] private float LookSensitivity = 0.2f;
    [SerializeField] private float LookAngleLimit = 90.0f;

    private Camera mainCamera;
    private CharacterController characterController;
    private Rigidbody rb;

    //dialogue 
    static public bool dialogue = false;

    private InputAction moveInput;
    // private InputAction jumpInput;

    private float currentMoveSpeed = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private float lookAngle = 0f;

    private Vector2 lookInput;
    private float xRotation = 0f;
    public bool freezePlayer;


    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        // jumpInput = InputSystem.actions.FindAction("Jump");

        // Making a variable so the cursor isn't shown on screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = WalkSpeed;
    }

    public void FreezePlayer()
    {
        freezePlayer = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Unfreeze()
    {
        freezePlayer = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Passing the movement vector as a parameter
        HandleMovement(moveVector); // Keeping the code clean and don't need to store every input as its own seperate field
        HandleLooking(mouseDelta);
    }

    // Creating a method to handle the overall movement
    private void HandleMovement(Vector2 moveVector)
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += Gravity * Time.deltaTime;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 horizontalMovement =
            forward * (moveVector.y * currentMoveSpeed) + right * (moveVector.x * currentMoveSpeed);

        moveDirection = horizontalMovement;
        moveDirection.y = verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        lookAngle += -mouseDelta.y * LookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -LookAngleLimit, LookAngleLimit);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * LookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Look action on controller triggered with controller");
            lookInput = context.ReadValue<Vector2>();
        }
    }
}