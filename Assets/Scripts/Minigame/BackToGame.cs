using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    public void ReturnToMainGame()
    {
        Scene quizScene = gameObject.scene;

        if (quizScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(quizScene);
        }
    }
}
