
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera = 10f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float fixedZ = 0f;

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mousePixelPos =
            Mouse.current.position.ReadValue();

        Vector3 mouseScreenPos = new Vector3(mousePixelPos.x, mousePixelPos.y, distanceFromCamera);

        Vector3 targetWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        // Keep the fish on the side-view plane.
        targetWorldPos.z = fixedZ;
        transform.position = Vector3.SmoothDamp(transform.position, targetWorldPos, ref velocity,smoothTime);
    }
}
