using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class SleepSystem : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject BlackPanel;

    [Header("Main Scene Triggers")]
    [SerializeField] private GameObject[] QuestTriggers;

    [Header("Day")]
    [SerializeField] private int CurrentDay = 1;
    [SerializeField] private int DaysToAdvance = 1;
    [SerializeField] private float WriteSpeed = 0.25f;
    [SerializeField] private DayNightManager dayNightManager;
    public int GameplayDay { get; private set; } = 1;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Subtext;
    [SerializeField] private GameObject[] uiToHideWhileSleeping;

    [Header("Objects to turn off when sleeping")]
    [SerializeField] private GameObject tvOnObject;

    private bool[] uiWasActive;
    private bool Caninteract = true;

    private void Start()
    {
        // The black panel and text should be hidden when the game starts.
        if (BlackPanel != null)
        {
            BlackPanel.SetActive(false);
        }

        if (Subtext != null)
        {
            Subtext.text = "";
        }
    }

    private void Update()
    {
        if (!Caninteract || interactAction == null || !interactAction.action.WasPressedThisFrame())
        {
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.CompareTag("Bed"))
            {
                if (!AreAllTasksComplete())
                {
                    Debug.Log("Finish your tasks before going to sleep!");
                    return;
                }
                // sleep
                StartCoroutine(SleepCO());
            }
        }
    }

    private void HideSleepUI()
    {
        uiWasActive = new bool[uiToHideWhileSleeping.Length];

        for (int i = 0; i < uiToHideWhileSleeping.Length; i++)
        {
            GameObject uiObject = uiToHideWhileSleeping[i];

            if (uiObject != null)
            {
                uiWasActive[i] = uiObject.activeSelf;
                uiObject.SetActive(false);
            }
        }
    }

    private void RestoreSleepUI()
    {
        if (uiWasActive == null)
        {
            return;
        }

        for (int i = 0; i < uiToHideWhileSleeping.Length; i++)
        {
            GameObject uiObject = uiToHideWhileSleeping[i];

            if (uiObject != null)
            {
                uiObject.SetActive(uiWasActive[i]);
            }
        }
    }

    IEnumerator SleepCO()
    {
        HideSleepUI();
        // Turn on the black panel while the next day is being shown.
        BlackPanel.SetActive(true);
        Caninteract = false;

        yield return new WaitForSeconds(1f);

        // Write the next day on the screen one letter at a time.
        Subtext.text = "";
        // Advance the day by the amount set in the Inspector.
        CurrentDay += DaysToAdvance;
        GameplayDay++;

        string Holder = "Day " + CurrentDay;

        foreach (char c in Holder)
        {
            Subtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }
        if (dayNightManager != null)
        {
            dayNightManager.ResetToFirstTimeStage();
        }
        // Wait before bringing the room back.
        yield return new WaitForSeconds(1f);
        //This resets the quests back to being available after sleeping.
        if (QuestManager.instance != null)
        {
            QuestManager.instance.ResetQuestsForNewDay();
        }
        // Turn the main scene's quest triggers back on for the new day.
        if (QuestTriggers != null)
        {
            foreach (GameObject trigger in QuestTriggers)
            {
                if (trigger != null)
                {
                    trigger.SetActive(true);
                }
            }
        }
        if (tvOnObject != null)
        {
            tvOnObject.SetActive(false);
        }
        // Clear the day text and turn off the black panel.
        Subtext.text = "";
        BlackPanel.SetActive(false);
        RestoreSleepUI();
        Caninteract = true;
    }
    private bool AreAllTasksComplete()
    {
        if (QuestManager.instance == null)
        {
            return false;
        }

        foreach (Quest quest in QuestManager.instance.AllQuests)
        {
            if (quest.info.isRequired && quest.state != QuestState.FINISHED)
            {
                return false;
            }
        }

        return true;
    }
}