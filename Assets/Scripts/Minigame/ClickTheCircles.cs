using UnityEngine;

public class ClickTheCircles : MonoBehaviour
{
    public GameObject minigameCanvas;
    public RectTransform circle;
    public RectTransform playArea;

    public int clicksNeeded = 5;

    private int clicks = 0;

    public void StartMinigame()
    {
        clicks = 0;

        minigameCanvas.SetActive(true);

        MoveCircle();
    }

    public void ClickCircle()
    {
        clicks++;

        if (clicks >= clicksNeeded)
        {
            EndMinigame();
        }
        else
        {
            MoveCircle();
        }
    }

    void MoveCircle()
    {
        float x = Random.Range(
            -playArea.rect.width / 2,
            playArea.rect.width / 2
        );

        float y = Random.Range(
            -playArea.rect.height / 2,
            playArea.rect.height / 2
        );

        circle.anchoredPosition = new Vector2(x, y);
    }

    void EndMinigame()
    {
        minigameCanvas.SetActive(false);
    }
}
