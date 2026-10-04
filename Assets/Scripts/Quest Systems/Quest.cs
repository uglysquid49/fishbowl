using UnityEngine;

public class Quest
{
    public QuestInfo info { get; private set; }
    public QuestState state { get; private set; }
    public int currentStepIndex { get; private set; }

    // tracking for current quest steeps will be added below;

    public Quest(QuestInfo questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.CAN_START;
    }

    public void ChangeState(QuestState newState)
    {
        this.state = newState;
    }

    public string GetPrefabTargetID()
    {
        return info.PrefabTargetID;
    }

    public void SetStepIndex(int index)
    {
        this.currentStepIndex = index;
    }
}

public enum QuestState
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED
}
