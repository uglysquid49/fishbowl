using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestListUI : MonoBehaviour
{
    [Header("Quest List UI")]
    [SerializeField] private Transform questListContainer;
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private GameObject questListPanel;

    private readonly List<GameObject> questEntries = new List<GameObject>();

    private Coroutine subscribeCoroutine;

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
        RefreshQuestList();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        if (subscribeCoroutine != null)
        {
            StopCoroutine(subscribeCoroutine);
        }

        if (GameEventsManager.instance != null && GameEventsManager.instance.questEvents != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= RefreshQuestList;
        }
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
        RefreshQuestList();
        UpdateQuestListVisibility();
    }

    private void RefreshQuestList(Quest changedQuest = null)
    {
        if (QuestManager.instance == null || questListContainer == null || questEntryPrefab == null)
            return;

        foreach (GameObject entry in questEntries)
        {
            Destroy(entry);
        }

        questEntries.Clear();

        foreach (Quest quest in QuestManager.instance.AllQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContainer);
            questEntries.Add(entry);

            TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>(true);

            if (texts.Length >= 2)
            {
                texts[0].text = quest.info.questName;
                texts[1].text = GetStatusText(quest.state);

                switch (quest.state)
                {
                    case QuestState.REQUIREMENTS_NOT_MET: texts[1].color = Color.gray;
                        break;

                    case QuestState.CAN_START: texts[1].color = Color.white;
                        break;

                    case QuestState.IN_PROGRESS: texts[1].color = Color.yellow;
                        break;

                    case QuestState.CAN_FINISH: texts[1].color = Color.cyan;
                        break;

                    case QuestState.FINISHED: texts[1].color = Color.green;
                        break;
                }
            }
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

        questListPanel.SetActive(!minigameIsLoaded);
    }
    private string GetStatusText(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                return "Locked";

            case QuestState.CAN_START:
                return "Can Start";

            case QuestState.IN_PROGRESS:
                return "In Progress";

            case QuestState.CAN_FINISH:
                return "Ready to Finish";

            case QuestState.FINISHED:
                return "Finished";

            default:
                return "Unknown";
        }
    }
}
