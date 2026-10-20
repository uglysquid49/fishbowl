using UnityEngine;

public class CustomMouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D mouseCursor;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        Cursor.SetCursor(mouseCursor, hotSpot, CursorMode.ForceSoftware);
    }
}
