using UnityEngine;

public class CustomMouse : MonoBehaviour
{
    // Attaching the design of the custom cursor
    public Texture2D cursor;

    private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        Cursor.SetCursor(cursor, hotSpot, cursorMode);
    }
}
