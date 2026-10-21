using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

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


    }
}
