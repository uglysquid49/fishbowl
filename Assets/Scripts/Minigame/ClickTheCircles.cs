using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickTheCircles : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private QuestInfo questInfo;

    public GameObject minigameCanvas;

    [Header("Circle Sprites")]
    [SerializeField] private Image circleImage;
    [SerializeField] private Sprite[] circleImageSprites;
    public RectTransform playArea;

    public int clicksNeeded = 5;
    public float spawnInterval = 1f;

    private int clicks = 0;

    public void StartMinigame()
    {
        clicks = 0;

        minigameCanvas.SetActive(true);

        MoveCircle();
    }

    public void ClickCircle()
    {

        Debug.Log("CLICKED!");
        clicks++;

        circleImage.gameObject.SetActive(false);


        if (clicks >= clicksNeeded)
        {
            EndMinigame();
        }
        else
        {
            StartCoroutine(SpawnNextCircle());
        }
    }
    IEnumerator SpawnNextCircle()
    {
        yield return new WaitForSeconds(spawnInterval);
        MoveCircle();
        circleImage.gameObject.SetActive(true);
    }

    void MoveCircle()
    {
        RectTransform circleRect = circleImage.rectTransform;

        float halfWidth = playArea.rect.width / 2f;
        float halfHeight = playArea.rect.height / 2f;
        float circleRadius = circleRect.rect.width / 2f;

        float x = Random.Range(-halfWidth + circleRadius, halfWidth - circleRadius);
        float y = Random.Range(-halfHeight + circleRadius, halfHeight - circleRadius);

        circleRect.anchoredPosition = new Vector2(x, y);

        // Choose a random sprite for this circle.
        if (circleImageSprites != null && circleImageSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, circleImageSprites.Length);
            circleImage.sprite = circleImageSprites[randomIndex];
        }
    }

    void EndMinigame()
    {
        minigameCanvas.SetActive(false);
        Debug.Log("Circle minigame completed!");

        // Mark the quest as finished
        if (questInfo != null && QuestManager.instance != null)
        {
            QuestManager.instance.CompleteAndCloseQuest(questInfo.id);
        }

        // Close only the minigame scene
        SceneManager.UnloadSceneAsync(gameObject.scene);
        Cursor.visible = false;
    }
}
