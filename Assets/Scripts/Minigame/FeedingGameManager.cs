using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FeedingGameManager : MonoBehaviour
{
    public static FeedingGameManager Instance;

    public TMP_Text scoreText;
    public GameObject winPanel;

    public int foodGoal = 10;

    private int foodEaten = 0;

    //tracks if this game is completed for data
    private bool isGameCompleted = false;

    void Awake()
    {
        Instance = this;
        //SceneManager.UnloadSceneAsync("DayScene3D");
    }

    void Start()
    {
        UpdateScore();
    }

    public void AddScore(int points)
    {
        foodEaten++;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = foodEaten + "/" + foodGoal;
        }
    }

    public void WinGame()
    {
        if (isGameCompleted)
        {
            return;
        }
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        isGameCompleted = true;

        PlayerPrefs.SetInt("FeedingGameCompleted", 1);
        PlayerPrefs.Save();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }
}
