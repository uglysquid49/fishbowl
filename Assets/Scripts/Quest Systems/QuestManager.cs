using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Rendering;

public class QuestManager : MonoBehaviour
{
    [Header("Persistence Configuration")]
    [SerializeField] private string saveFileName = "rpg_prefab_quest_save_data";

    public static QuestManager instance { get; private set; }

    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeQuestMap();
        LoadQuestSystemState();
    }

    private void InitializeQuestMap()
    {
        QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");

        foreach (QuestInfo info in allQuests)
        {
            if (questMap.ContainsKey(info.id)) continue;
            questMap.Add(info.id, new Quest(info));
        }
    }

    public void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.CAN_START)
        {
            quest.ChangeState(QuestState.IN_PROGRESS);

            if (!string.IsNullOrEmpty(quest.GetPrefabTargetID()) && SceneMiniGameRegistry.instance != null)
            {
                SceneMiniGameRegistry.instance.ToggleMiniGameHierarchy(quest.GetPrefabTargetID(), true);
            }

            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.questEvents.QuestStateChanged(quest);
            }

            SaveQuestSystemState();
        }
    }

    public void CompleteAndCloseQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.IN_PROGRESS)
        {
            quest.ChangeState(QuestState.FINISHED);

            if (!string.IsNullOrEmpty(quest.GetPrefabTargetID()) && SceneMiniGameRegistry.instance != null)
            {
                SceneMiniGameRegistry.instance.ToggleMiniGameHierarchy(quest.GetPrefabTargetID(), false);
            }

            SaveQuestSystemState();
            Debug.Log($"Quest {id} marked finished. Mini-game hidden successfully.");
        }
    }

    private Quest GetQuestById(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest)) return quest;
        return null;
    }

    public void SaveQuestSystemState()
    {
        GameSaveDataWrapper wrapper = new GameSaveDataWrapper();

        foreach (KeyValuePair<string, Quest> pair in questMap)
        {
            QuestDataSave savedData = new QuestDataSave
            {
                questId = pair.Key,
                state = pair.Value.state,
                currentQuestStepIndex = pair.Value.currentStepIndex
            };
            wrapper.savedQuests.Add(savedData);
        }

        string jsonOutput = JsonUtility.ToJson(wrapper, true);
        PlayerPrefs.SetString(saveFileName, jsonOutput);
        PlayerPrefs.Save();
    }

    public void LoadQuestSystemState()
    {
        if (!PlayerPrefs.HasKey(saveFileName)) return;

        string rawJson = PlayerPrefs.GetString(saveFileName);
        GameSaveDataWrapper wrapper = JsonUtility.FromJson<GameSaveDataWrapper>(rawJson);

        foreach (QuestDataSave savedQuestData in wrapper.savedQuests)
        {
            Quest liveQuest = GetQuestById(savedQuestData.questId);
            if (liveQuest != null)
            {
                liveQuest.ChangeState(savedQuestData.state);
                liveQuest.SetStepIndex(savedQuestData.currentQuestStepIndex);

                if (liveQuest.state == QuestState.IN_PROGRESS && !string.IsNullOrEmpty(liveQuest.GetPrefabTargetID()))
                {
                    StartCoroutine(ExecuteDelayedPrefabRecovery(liveQuest.GetPrefabTargetID()));
                }
            }
        }
    }

    private System.Collections.IEnumerator ExecuteDelayedPrefabRecovery(string prefabID)
    {
        yield return new WaitForEndOfFrame();
        if (SceneMiniGameRegistry.instance != null)
        {
            SceneMiniGameRegistry.instance.ToggleMiniGameHierarchy(prefabID, true);
        }
    }
}

[System.Serializable]
public struct QuestDataSave
{
    public string questId;
    public QuestState state;
    public int currentQuestStepIndex;
}

[System.Serializable]
public class GameSaveDataWrapper
{
    public List<QuestDataSave> savedQuests = new List<QuestDataSave>();
}

