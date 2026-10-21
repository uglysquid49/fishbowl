
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuestCanvasUI : MonoBehaviour
{
    [Header("UI Visual Panels")]
    [SerializeField] private GameObject questObjectiveGuidePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject questListPanel;

    [SerializeField] private CanvasGroup questObjectiveGuideCanvasGroup;
    [SerializeField] private float fadeDuration = 0.75f;
    [SerializeField] private float completionMessageDuration = 1.5f;

    private Coroutine objectiveGuideCoroutine;
    private void OnEnable()
    {
        // Connecting to the global quest event system
        if (GameEventsManager.instance != null && GameEventsManager.instance.questEvents != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= UpdateQuestUI;
            GameEventsManager.instance.questEvents.onQuestStateChange += UpdateQuestUI;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // disconnecting from global system to protect memory stability and prevent errors
        if (GameEventsManager.instance != null && GameEventsManager.instance.questEvents != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= UpdateQuestUI;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateObjectiveGuide();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (IsMinigameScene(scene.name))
        {
            if (objectiveGuideCoroutine != null)
                StopCoroutine(objectiveGuideCoroutine);

            objectiveGuideCoroutine = StartCoroutine(ShowCompletionThenNextQuest());
            return;
        }

        UpdateObjectiveGuide();
    }

    private void Start()
    {
        UpdateObjectiveGuide();
    }

    private bool IsMinigameScene(string sceneName)
    {
        return sceneName == "ClickCircles" ||
               sceneName == "FeedingFishy" ||
               sceneName == "QuizBowl";
    }

    private IEnumerator ShowCompletionThenNextQuest()
    {
        if (questListPanel != null)
            questListPanel.SetActive(false);

        if (questObjectiveGuidePanel == null || statusText == null)
            yield break;

        questObjectiveGuidePanel.SetActive(true);
        statusText.text = "Complete!";

        if (questObjectiveGuideCanvasGroup != null)
            questObjectiveGuideCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(completionMessageDuration);

        if (questObjectiveGuideCanvasGroup != null)
            yield return FadeObjectiveGuide(1f, 0f);

        UpdateObjectiveGuide();

        if (questObjectiveGuideCanvasGroup != null && questObjectiveGuidePanel.activeSelf)
            yield return FadeObjectiveGuide(0f, 1f);

        objectiveGuideCoroutine = null;
    }

    private IEnumerator FadeObjectiveGuide(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        questObjectiveGuideCanvasGroup.alpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            questObjectiveGuideCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            yield return null;
        }

        questObjectiveGuideCanvasGroup.alpha = targetAlpha;
    }
    private bool IsMinigameLoaded()
    {
        return SceneManager.GetSceneByName("ClickCircles").isLoaded ||
               SceneManager.GetSceneByName("FeedingFishy").isLoaded ||
               SceneManager.GetSceneByName("QuizBowl").isLoaded;
    }

    private void UpdateQuestUI(Quest quest)
    {
        UpdateObjectiveGuide();
    }

    private void UpdateObjectiveGuide()
    {
        if (questObjectiveGuidePanel == null)
            return;

        if (IsMinigameLoaded() || QuestManager.instance == null)
        {
            questObjectiveGuidePanel.SetActive(false);
            return;
        }

        Quest activeQuest = null;

        // Find a quest that is already in progress first
        foreach (Quest quest in QuestManager.instance.AllQuests)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                activeQuest = quest;
                break;
            }
        }

        // If no quest is in progress, find the first available quest
        if (activeQuest == null)
        {
            foreach (Quest quest in QuestManager.instance.AllQuests)
            {
                if (quest.state == QuestState.CAN_START)
                {
                    activeQuest = quest;
                    break;
                }
            }
        }

        if (activeQuest == null)
        {
            questObjectiveGuidePanel.SetActive(false);
            return;
        }

        titleText.text = activeQuest.info.questName;

        if (activeQuest.state == QuestState.IN_PROGRESS)
        {
            statusText.text = "In Progress";
        }
        else
        {
            statusText.text = "Can Start";
        }

        questObjectiveGuidePanel.SetActive(true);
    }
}
