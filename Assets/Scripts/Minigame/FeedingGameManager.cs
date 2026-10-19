
using TMPro;
using UnityEngine;

public class FeedingGameManager : MonoBehaviour
{
    public static FeedingGameManager Instance;

    public TMP_Text scoreText;
    public TMP_Text gameOverText;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScore();

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
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
            scoreText.text = "Score: " + score;
        }
    }

    public void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER!";
        }
    }
}
