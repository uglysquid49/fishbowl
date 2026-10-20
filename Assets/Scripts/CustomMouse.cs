using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.LowLevel;

public class CustomMouse : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] public Texture2D cursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    [Header("Controller Settings")]
    [SerializeField] private float cursorSpeed = 1000f; // Counting pixels per second

    //private CursorMode cursorMode = CursorMode.Auto;

    private Vector2 virtualMousePosition;
    private bool isUsingController = false;

    void Start()
    {
        // Setting the custom cursor hardware
        Cursor.SetCursor(cursor, hotSpot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;

        // Tracking will start at center of screen through this line;
        virtualMousePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        HandleMovement();
        HandleClicks();
    }

    private void HandleMovement()
    {
        if (Mouse.current == null) return;

        Vector2 joystickInput = Vector2.zero;
        Vector2 mouseDelta = Vector2.zero;

        if (Gamepad.current != null) joystickInput = Gamepad.current.leftStick.ReadValue();
        mouseDelta = Mouse.current.delta.ReadValue();

        // Detecting what device the user will be using
        if (joystickInput.sqrMagnitude > 0.05f)
        {
            isUsingController = true;
        }
        else if (mouseDelta.sqrMagnitude > 0.1f)
        {
            isUsingController = false;
        }

        // Movement being applied BUT based on which controller is being used at moment
        if (isUsingController)
        {
            // The position of the virtual mouse via joystick
            virtualMousePosition += joystickInput * cursorSpeed * Time.deltaTime;
            virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
            virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);

            Mouse.current.WarpCursorPosition(virtualMousePosition);
        }
        else
        {
            virtualMousePosition = Mouse.current.position.ReadValue();
        }
    }

    private void HandleClicks()
    {
        if (Gamepad.current == null || Mouse.current == null) return;

        // Checking if the "primary" button on controllers = the south button is pressed
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            InputSystem.QueueButtonPressEvent(Mouse.current.leftButton, true);
        }
        else if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            InputSystem.QueueButtonPressEvent(Mouse.current.leftButton, false);
        }
    }

    void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
    }
}
