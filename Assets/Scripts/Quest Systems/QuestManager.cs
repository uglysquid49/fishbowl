using System.Collections.Generic;
using System.Globalization;
using UnityEngine.SceneManagement;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance {  get; private set; }

    // Making a private dictionary that maps a string to a quest called QuestMap.
    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // crucial for persistent panagers across scenes

        InitializeQuestMap();
    }

    private void InitializeQuestMap()
    {
        // Loading all QuestInfo assets from Resources folder
        QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
        foreach (QuestInfo info in allQuests)
        {
            if (questMap.ContainsKey(info.id))
            {
                Debug.LogWarning($"Duplicate quest ID found: {info.id}");
            }
            questMap.Add(info.id, new Quest(info));
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.CAN_START)
        {
            quest.ChangeState(QuestState.IN_PROGRESS);

            // Trigger Scene Change if target scene is defined
            if (!string.IsNullOrEmpty(quest.GetTargetScene()))
            {
                LoadQuestScene(quest.GetTargetScene());
            }

            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void LoadQuestScene(string sceneName)
    {
        Debug.Log($"Loading Scene for Quest: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    private Quest GetQuestById(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest)) return quest;
        return null;
    }
}
