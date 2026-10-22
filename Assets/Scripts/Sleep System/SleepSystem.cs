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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Subtext;

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
                // sleep
                StartCoroutine(SleepCO());
            }
        }
    }

    IEnumerator SleepCO()
    {
        // Turn on the black panel while the next day is being shown.
        BlackPanel.SetActive(true);
        Caninteract = false;

        yield return new WaitForSeconds(1f);

        // Write the next day on the screen one letter at a time.
        Subtext.text = "";
        CurrentDay++;
        // Advance the day by the amount set in the Inspector.
        CurrentDay += DaysToAdvance;
        string Holder = "Day " + CurrentDay;

        foreach (char c in Holder)
        {
            Subtext.text += c;
            yield return new WaitForSeconds(WriteSpeed);
        }

        // Wait before bringing the room back.
        yield return new WaitForSeconds(1f);

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

        // Clear the day text and turn off the black panel.
        Subtext.text = "";
        BlackPanel.SetActive(false);
        Caninteract = true;
    }
}