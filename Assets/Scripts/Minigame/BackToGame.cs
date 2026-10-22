using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    [SerializeField] private QuestInfo questInfo;

    public void ReturnToMainGame()
    {
        Time.timeScale = 1f;

        Scene scene = gameObject.scene;
        string sceneName = scene.name;

        if (QuestManager.instance != null)
        {
            foreach (Quest quest in QuestManager.instance.AllQuests)
            {
                if (quest.state == QuestState.IN_PROGRESS && quest.GetPrefabTargetID() == sceneName)
                {
                    Cursor.visible = false;
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

        // Turn off cursor
        Cursor.visible = false;
    }
}