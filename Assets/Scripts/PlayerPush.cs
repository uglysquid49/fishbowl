using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDoorPush : MonoBehaviour
{
    public float pushForce = 3f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;

        // Push the door in the direction the player is moving into it.
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        if (pushDirection.sqrMagnitude > 0.001f)
        {
            body.AddForceAtPosition(pushDirection.normalized * pushForce, hit.point, ForceMode.Force);
        }
    }
}