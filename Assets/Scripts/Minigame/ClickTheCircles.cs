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
        float halfWidth = playArea.rect.width / 2f;
        float halfHeight = playArea.rect.height / 2f;
        float circleRadius = circle.rect.width / 2f;

        float x = Random.Range(-halfWidth + circleRadius, halfWidth - circleRadius);
        float y = Random.Range(-halfHeight + circleRadius, halfHeight - circleRadius);

        circle.anchoredPosition = new Vector2(x, y);
    }

    void EndMinigame()
    {
        minigameCanvas.SetActive(false);
        Debug.Log("wow you can click");
    }
}
