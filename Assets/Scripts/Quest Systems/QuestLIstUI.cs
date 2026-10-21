using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestListUI : MonoBehaviour
{
    [Header("Quest List UI")]
    [SerializeField] private Transform questListContainer;
    [SerializeField] private GameObject questEntryPrefab;

    private readonly List<GameObject> questEntries = new List<GameObject>();

    private void OnEnable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange += RefreshQuestList;
        }
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= RefreshQuestList;
        }
    }

    private void Start()
    {
        RefreshQuestList();
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
