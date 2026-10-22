using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    [SerializeField] private QuestInfo questInfo;

    public void ReturnToMainGame()
    {
        Scene scene = gameObject.scene;
        string sceneName = scene.name;

        if (QuestManager.instance != null)
        {
            // Use the assigned quest if there is one.
            if (questInfo != null)
            {
                QuestManager.instance.CompleteAndCloseQuest(questInfo.id);
                return;
            }

            // Otherwise, find the in-progress quest for this minigame scene.
            foreach (Quest quest in QuestManager.instance.AllQuests)
            {
                if (quest.state == QuestState.IN_PROGRESS && quest.GetPrefabTargetID() == sceneName)
                {
                    QuestManager.instance.CompleteAndCloseQuest(quest.info.id);
                    return;
                }
            }
        } 

        // Keep the Back button usable if the quest could not be found.
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}