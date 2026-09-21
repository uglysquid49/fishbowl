using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions3D : MonoBehaviour
{
    public Transform orientation;
    
    // Draw a racast so player can recieve quests from fish NPC
    void Start()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Drawing the ray projection
        Debug.DrawRay(transform.position, orientation.forward * 4, Color.blue);
        
        // if there is a quest giver (the fish) and we have pressed e
        if (RaycastInteraction() != null && context.performed)
        {
            RaycastInteraction().Talk(this);
            return;
        }
    }

    QuestGiver RaycastInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, orientation.forward, out hit, 4f))
        {
            return hit.collider.gameObject.GetComponent<QuestGiver>();
        }
        else
            return null;
    }
}
