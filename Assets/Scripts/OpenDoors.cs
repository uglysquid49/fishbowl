using UnityEngine;

public class PhysicsPush : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushPower = 3.0f;
    [SerializeField] private float weightThreshold = -0.3f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;
        if (hit.moveDirection.y < weightThreshold) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForceAtPosition(pushDir * pushPower, hit.point, ForceMode.Impulse);
    }
}
