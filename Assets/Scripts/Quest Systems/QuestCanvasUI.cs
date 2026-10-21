using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestCanvasUI : MonoBehaviour
{
    [Header("UI Visual Panels")]
    [SerializeField] private GameObject questNotificationPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statusText;

    private void OnEnable()
    {
        // Connecting to the global quest event system
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange += UpdateQuestUI;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // disconnecting from global system to protect memory stability and prevent errors
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= UpdateQuestUI;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        CancelInvoke(nameof(HideNotificationPanel));
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CancelInvoke(nameof(HideNotificationPanel));
        questNotificationPanel.SetActive(false);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        CancelInvoke(nameof(HideNotificationPanel));
        questNotificationPanel.SetActive(false);
    }

    private void Start()
    {
        // Starting with UI notification display hidden
        questNotificationPanel.SetActive(false);
    }
    private bool IsMinigameLoaded()
    {
        return UnityEngine.SceneManagement.SceneManager.GetSceneByName("ClickCircles").isLoaded ||
               UnityEngine.SceneManagement.SceneManager.GetSceneByName("FeedingFishy").isLoaded ||
               UnityEngine.SceneManagement.SceneManager.GetSceneByName("QuizBowl").isLoaded;
    }
    private void UpdateQuestUI(Quest quest)
    {
        CancelInvoke(nameof(HideNotificationPanel));

        if (IsMinigameLoaded())
        {
            questNotificationPanel.SetActive(false);
            return;
        }
        // This will handle the displaying of info based on state changes
        if (quest.state == QuestState.IN_PROGRESS)
        {
            titleText.text = quest.info.questName;
            statusText.text = "Mini-Game Active";
            questNotificationPanel.SetActive(true);
        }
        else if (quest.state == QuestState.FINISHED)
        {
            titleText.text = quest.info.questName;
            statusText.text = "Mini-Game Cleared!";

            // Hiding the window after 3 seconds through a internal delay call
            Invoke(nameof(HideNotificationPanel), 3f);
        }
    }

    private void HideNotificationPanel()
    {
        questNotificationPanel.SetActive(false);
    }
}
