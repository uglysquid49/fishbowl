using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

public class CustomMouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D mouseCursor;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    public float controllerSpeed = 500f;

    [Header("UI References")]
    public RectTransform cursorUIElement; // The UI Image goes into this slot
    public Canvas scalerCanvas; // Sliding the canvas object here

    private Vector2 simulatedMousePosition;
    private bool isUsingGamepad = false;

    private void Awake()
    {
        Cursor.SetCursor(mouseCursor, hotSpot, CursorMode.ForceSoftware);
    }

    private void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (cursorUIElement != null && mouseCursor != null)
        {
            Image cursorImage = cursorUIElement.GetComponent<Image>();
            if (cursorImage != null)
            {
                Rect rect = new Rect (0, 0, mouseCursor.width, mouseCursor.height);
                Sprite runtimeSprite = Sprite.Create(mouseCursor, rect, new Vector2(0f, 1f));
                cursorImage.sprite = runtimeSprite;
            }

            cursorUIElement.pivot = new Vector2(0f, 1f);
        }

        if (Mouse.current != null)
        {
            simulatedMousePosition = Mouse.current.position.ReadValue();
        }
        else
        {
            simulatedMousePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }
    }

    private void Update()
    {
        HandleInputDetection();

        if (isUsingGamepad)
        {
            HandleHardwareFreeMovement();
            HandleControllerAndKeyboardSelection();
        }
        else
        {
            HandleHardwareMouseMovement();
        }

        UpdateCursorUIPosition();
    }

    private void HandleInputDetection()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.magnitude > 0.01f)
            {
                isUsingGamepad = false;
                return;
            }
        }

        if (Gamepad.current != null)
        {
            Vector2 stickInput = Gamepad.current.leftStick.ReadValue();
            if (stickInput.magnitude > 0.1f)
            {
                isUsingGamepad = true;
                return;
            }
        }

        if (Keyboard.current != null)
        {
            bool wasdPressed = Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
                              Keyboard.current.sKey.isPressed ||
                              Keyboard.current.dKey.isPressed ||
                              Keyboard.current.upArrowKey.isPressed ||
                              Keyboard.current.downArrowKey.isPressed ||
                              Keyboard.current.leftArrowKey.isPressed ||
                              Keyboard.current.rightArrowKey.isPressed;
            if (wasdPressed)
            {
                isUsingGamepad = true;
            }
        }
    }

    private void HandleHardwareMouseMovement()
    {
        if (Mouse.current != null)
        {
            // directly follow desktop mouse
            simulatedMousePosition = Mouse.current.position.ReadValue();
        }
    }

    private void HandleHardwareFreeMovement()
    {
        Vector2 movementDirection = Vector2.zero;

        // Reading controller left stick
        if (Gamepad.current != null)
        {
            movementDirection = Gamepad.current.leftStick.ReadValue();
        }

        // Reading keyboard inputs if no controller is detected
        if (movementDirection.magnitude < 0.1f && Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) movementDirection.y = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) movementDirection.y = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movementDirection.x = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movementDirection.x = 1f;

            movementDirection.Normalize();
        }

        // Moving coordinates based on time
        simulatedMousePosition += movementDirection * controllerSpeed * Time.deltaTime;

        simulatedMousePosition.x = Mathf.Clamp(simulatedMousePosition.x, 0f, Screen.width);
        simulatedMousePosition.y = Mathf.Clamp(simulatedMousePosition.y, 0f, Screen.height);
    }

    private void UpdateCursorUIPosition()
    {
        if (cursorUIElement == null || scalerCanvas == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scalerCanvas.transform as RectTransform,
            simulatedMousePosition - hotSpot,
            scalerCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
            );

        cursorUIElement.position = localPoint;
    }

    private void HandleControllerAndKeyboardSelection()
    {
        bool isSouthButtonPressed = false;
        bool isLeftMousePressed = false;

        if (Gamepad.current != null)
        {
            isSouthButtonPressed = Gamepad.current.buttonSouth.isPressed;
        }

        if (Mouse.current != null)
        {
            isLeftMousePressed = Mouse.current.leftButton.isPressed;
        }
    }
}
