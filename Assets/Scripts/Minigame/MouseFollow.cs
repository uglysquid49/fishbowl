using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera = 10f;
    [SerializeField] private float smoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePixelPos = Mouse.current.position.ReadValue();

        Vector3 mouseScreenPos = new Vector3(mousePixelPos.x, mousePixelPos.y, distanceFromCamera);

        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        transform.position = Vector3.SmoothDamp(transform.position, targetWorldPos, ref velocity, smoothTime);
    }
}
