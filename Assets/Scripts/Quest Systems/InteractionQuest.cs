using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionQuest : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private string playerTag = "Player";

    [Header("TV")]
    [SerializeField] private GameObject tvOnObject;

    [Header("Quest")]
    [SerializeField] private QuestInfo watchingTVQuest;

    private bool playerInRange;
    private bool used;

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        if (!playerInRange || used || interactAction == null)
        {
            return;
        }

        if (interactAction.action.WasPressedThisFrame())
        {
            // Turn on the TV.
            if (tvOnObject != null)
            {
                tvOnObject.SetActive(true);
            }

            // Start the quest if it hasn't been started yet, then complete it.
            if (watchingTVQuest != null && QuestManager.instance != null)
            {
                QuestManager.instance.StartQuest(watchingTVQuest.id);
                QuestManager.instance.CompleteAndCloseQuest(watchingTVQuest.id);
            }
            else
            {
                Debug.LogWarning("Watching TV QuestInfo or QuestManager is missing.");
            }

            used = true;

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false);
            }
        }
    }
}