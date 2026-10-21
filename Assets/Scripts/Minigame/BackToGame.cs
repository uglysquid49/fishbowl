using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    [SerializeField] private QuestInfo questInfo;

    public void ReturnToMainGame()
    {
        if (questInfo != null && QuestManager.instance != null)
        {
            QuestManager.instance.CompleteAndCloseQuest(questInfo.id);
        }

        Scene scene = gameObject.scene;
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}