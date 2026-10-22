using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestPoint3D : MonoBehaviour
{
    [Header("Quest Configuration Links")]
    [SerializeField] private QuestInfo questInfoAsset;
    [SerializeField] string miniGameName;

    [Header("Cursor Settings")]
    public PlayerController3D playerController3D;

    [Header("Trigger Tag Validation")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        if (questInfoAsset == null || QuestManager.instance == null)
        {
            Debug.LogWarning("QuestInfo or QuestManager is missing.");
            return;
        }

        QuestManager.instance.StartQuest(questInfoAsset.id);
        Debug.Log($"3D Trigger fired for quest asset: {questInfoAsset.questName}");

        // if no minigame, let the player move around and leave the trigger active.
        if (string.IsNullOrWhiteSpace(miniGameName))
        {
            return;
        }

        // if theres minigame, freeze the player and open the minigame additively.
        if (playerController3D != null)
        {
            playerController3D.FreezePlayer();
        }

        SceneManager.LoadScene(miniGameName, LoadSceneMode.Additive);
        gameObject.SetActive(false);
    }
}
