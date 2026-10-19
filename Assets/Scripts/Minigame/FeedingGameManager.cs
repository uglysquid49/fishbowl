using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedingGameManager : MonoBehaviour
{
    public static FeedingGameManager Instance;

    public TMP_Text scoreText;
    public GameObject winPanel;

    private int score = 0;

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
        score += points;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Food eaten: " + score;
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
