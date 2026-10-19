using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    public float fallSpeed = 1f;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}