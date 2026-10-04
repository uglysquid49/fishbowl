using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public InputActionReference lookActionReference;
    public Transform playerBody;
    [Range(0.01f, 1f)] public float mouseSensitivity = 0.1f;
    public float gamepadSensitivity = 120f;

    private float xRotation = 0f;

    void LateUpdate()
    {
        Vector2 lookInput = lookActionReference.action.ReadValue<Vector2>();

        bool isGamepad = lookActionReference.action.activeControl?.device is Gamepad;

        float moveX, moveY;

        if (isGamepad)
        {
            moveX = lookInput.x * gamepadSensitivity * Time.deltaTime;
            moveY = lookInput.y * gamepadSensitivity * Time.deltaTime;
        }
        else
        {
            moveX = lookInput.x * mouseSensitivity;
            moveY = lookInput.y * mouseSensitivity;
        }

        xRotation -= moveY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * moveX);
    }
}
