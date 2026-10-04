using UnityEngine;

public class QuestPoint3D : MonoBehaviour
{
    [Header("Quest Configuration Links")]
    [SerializeField] private QuestInfo questInfoAsset;

    [Header("Trigger Tag Validation")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (questInfoAsset != null && QuestManager.instance != null)
            {
                QuestManager.instance.StartQuest(questInfoAsset.id);

                Debug.Log($"3D Trigger fired for quest asset: {questInfoAsset.questName}");

                gameObject.SetActive(false);
            }
        }

        Debug.Log($"Something touched the trigger zone: {other.gameObject.name}");
    }
}
