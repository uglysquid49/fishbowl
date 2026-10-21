using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickTheCircles : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private QuestInfo questInfo;

    public GameObject minigameCanvas;
    public RectTransform circle;
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

        circle.gameObject.SetActive(false);


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
        circle.gameObject.SetActive(true);
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
