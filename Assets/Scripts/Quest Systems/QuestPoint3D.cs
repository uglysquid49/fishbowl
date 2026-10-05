using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestPoint3D : MonoBehaviour
{
    [Header("Quest Configuration Links")]
    [SerializeField] private QuestInfo questInfoAsset;
    [SerializeField] string miniGameName;

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
                SceneManager.LoadScene(miniGameName, LoadSceneMode.Additive);
                //Camera.main.gameObject.SetActive(false); // Disabling the 3d camera
                gameObject.SetActive(false);
            }
        }

        Debug.Log($"Something touched the trigger zone: {other.gameObject.name}");

    }
}
