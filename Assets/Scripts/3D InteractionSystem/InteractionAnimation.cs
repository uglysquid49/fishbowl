using UnityEngine;

public class InteractionAnimation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float bobIntensity = 0.2f;
    public float bobSpeed = 3f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. Spin the 3D model around its Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        // 2. Make it gently bob up and down using a sine wave
        float newY = startPosition.y + (Mathf.Sin(Time.time * bobSpeed) * bobIntensity);
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
