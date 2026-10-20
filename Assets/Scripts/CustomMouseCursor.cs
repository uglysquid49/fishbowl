using UnityEngine;

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
}
