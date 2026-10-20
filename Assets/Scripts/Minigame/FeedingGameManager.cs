using TMPro;
using UnityEngine;

public class FeedingGameManager : MonoBehaviour
{
    public static FeedingGameManager Instance;

    public TMP_Text scoreText;
    public GameObject winPanel;

    public int foodGoal = 10;

    private int foodEaten = 0;

    void Awake()
    {
        Instance = this;
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
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
