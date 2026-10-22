using UnityEngine;

public class TVRemoteQuest : MonoBehaviour
{
    [SerializeField] private QuestInfo watchingTVQuest;
    [SerializeField] private GameObject tvOnObject;

    public void UseRemote()
    {
        if (tvOnObject != null)
        {
            tvOnObject.SetActive(true);
        }

        if (watchingTVQuest != null && QuestManager.instance != null)
        {
            QuestManager.instance.CompleteAndCloseQuest(watchingTVQuest.id);
        }
    }
}