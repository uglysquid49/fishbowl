using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class InteractionQuest : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private string playerTag = "Player";

    [Header("TV")]
    [SerializeField] private GameObject tvOnObject;
    [SerializeField] private VideoPlayer tvVideoPlayer;

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
            // Turn on the TV object.
            if (tvOnObject != null)
            {
                tvOnObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Assign the TV screen object to TV On Object.");
            }

            // Start playing the TV video.
            if (tvVideoPlayer != null)
            {
                tvVideoPlayer.gameObject.SetActive(true);
                tvVideoPlayer.Play();
            }

            // Complete the Watching TV quest.
            if (watchingTVQuest != null && QuestManager.instance != null)
            {
                QuestManager.instance.StartQuest(watchingTVQuest.id);
                QuestManager.instance.CompleteAndCloseQuest(watchingTVQuest.id);
            }

            used = true;

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false);
            }
        }
    }

    public void ResetForNewDay()
    {
        used = false;

        if (interactionIndicator != null)
        {
            interactionIndicator.SetActive(playerInRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;

            if (!used && interactionIndicator != null)
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