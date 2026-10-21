using TMPro;
using UnityEngine;

public class FeedingGameManager : MonoBehaviour, IDataPersistence
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
    }

    void Start()
    {
        UpdateScore();
    }

    public void LoadData(GameData data)
    {
        this.isGameCompleted = data.feedingGameCompleted;
        if (this.isGameCompleted)
        {
            Debug.Log("Feeding freenzy OVER!!");
        }
    }
    public void SaveData(ref GameData data)
    {
        data.feedingGameCompleted = this.isGameCompleted;
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
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        isGameCompleted = true;
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.SaveGame();
            Debug.Log("game complete ! progresssed saved");
        }
        Time.timeScale = 0f;
    }
}
