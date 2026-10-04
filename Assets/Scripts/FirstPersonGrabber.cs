using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonGrabber : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float grabDistance = 5f;
    [SerializeField] private LayerMask grabLayer;

    [Header("Hold Settings")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float attractionSpeed = 10f;

    private Rigidbody grabbedObject;

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("E key pressed!");
            
            if (grabbedObject != null)
            {
                DropObject();
            }
            else
            {
                TryGrabObject();
            }
        }
    }

    private void TryGrabObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(transform.position, transform.forward * grabDistance, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabLayer))
        {
            Debug.Log($"Raycast HIT something! Object Name: {hit.collider.gameObject.name}");
            
            if (hit.collider.TryGetComponent(out Rigidbody rb))
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;

                grabbedObject.linearDamping = 10f;
                grabbedObject.angularDamping = 10f;

                Debug.Log($"Successful Grabbed: {grabbedObject.name}");
            }
            else
            {
                Debug.LogWarning($"Hit {hit.collider.gameObject.name}, but is missing a rb component");
            }
        }
        else
        {
            Debug.LogWarning("Raycast has hit nothing");
        }
    }

    private void DropObject()
    {
        if (grabbedObject == null) return;

        grabbedObject.useGravity = true;
        grabbedObject.linearDamping = 0f;
        grabbedObject.angularDamping = 0.05f;

        grabbedObject = null;
    }

    private void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            Vector3 direction = holdPosition.position - grabbedObject.position;
            grabbedObject.linearVelocity = direction * attractionSpeed;
        }
    }
}
