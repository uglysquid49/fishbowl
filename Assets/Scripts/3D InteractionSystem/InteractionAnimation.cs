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
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        float newY = startPosition.y + (Mathf.Sin(Time.time * bobSpeed) * bobIntensity);
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
