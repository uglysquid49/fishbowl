using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestListUI : MonoBehaviour
{
    [Header("Quest List UI")]
    [SerializeField] private GameObject questListPanel;
    [SerializeField] private TextMeshProUGUI questListText;

    [Header("Quest Menu Controls")]
    [SerializeField] private KeyCode questMenuKey = KeyCode.J;

    private Coroutine subscribeCoroutine;

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            if (questListPanel == null)
                return;

            bool minigameIsLoaded =
                SceneManager.GetSceneByName("ClickCircles").isLoaded ||
                SceneManager.GetSceneByName("FeedingFishy").isLoaded ||
                SceneManager.GetSceneByName("QuizBowl").isLoaded;

            if (minigameIsLoaded)
                return;

            bool showQuestList = !questListPanel.activeSelf;

            if (showQuestList)
                RefreshQuestList();

            questListPanel.SetActive(showQuestList);
        }
    }

    private void OnEnable()
    {
        subscribeCoroutine = StartCoroutine(SubscribeToQuestEvents());
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private IEnumerator SubscribeToQuestEvents()
    {
        while (GameEventsManager.instance == null || GameEventsManager.instance.questEvents == null)
        {
            yield return null;
        }

        GameEventsManager.instance.questEvents.onQuestStateChange -= RefreshQuestList;
        GameEventsManager.instance.questEvents.onQuestStateChange += RefreshQuestList;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        if (subscribeCoroutine != null)
            StopCoroutine(subscribeCoroutine);

        if (GameEventsManager.instance != null && GameEventsManager.instance.questEvents != null)
            GameEventsManager.instance.questEvents.onQuestStateChange -= RefreshQuestList;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateQuestListVisibility();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        UpdateQuestListVisibility();
    }

    private void Start()
    {
        if (questListPanel != null)
            questListPanel.SetActive(false);

        RefreshQuestList();
    }

    private void RefreshQuestList(Quest changedQuest = null)
    {
        if (questListText == null || QuestManager.instance == null)
            return;

        questListText.text = "";

        foreach (Quest quest in QuestManager.instance.AllQuests)
        {
            string questStatus = quest.state == QuestState.IN_PROGRESS
                ? "In Progress"
                : quest.state == QuestState.CAN_START
                    ? "Available"
                    : quest.state.ToString();

            questListText.text += quest.info.questName + " - " + questStatus + "\n";
        }
    }

    private void UpdateQuestListVisibility()
    {
        if (questListPanel == null)
            return;

        bool minigameIsLoaded =
            SceneManager.GetSceneByName("ClickCircles").isLoaded ||
            SceneManager.GetSceneByName("FeedingFishy").isLoaded ||
            SceneManager.GetSceneByName("QuizBowl").isLoaded;

        if (minigameIsLoaded)
            questListPanel.SetActive(false);
    }
}