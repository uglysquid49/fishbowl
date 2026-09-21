using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            // Need to Do ; Advance the quest forward now that this step is finished
            //Destroy(this.gameObject);
        }
    }
}
