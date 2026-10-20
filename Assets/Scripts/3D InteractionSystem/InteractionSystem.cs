using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem instance;

    bool player_detection = false;

    [Header("Dialogue Lines")]
    public DialogueLine[] currentDialogue;

    [SerializeField]
    private InputActionReference actionReference;
     
    [Header("Linked Components for Interactions")]
    public TextMeshProUGUI textBox;
    public GameObject dialogueUI;
    public GameObject canva;
    private GameObject currentDialogueUI;
    private bool isItemDialogue = false;

    [Header("Text Configuration")]
    public float typingSpeed = 0.05f;

    [Header("Dialogue Status")]
    public bool isTyping = false;
    public bool dialogueFinished = true;

    [Header("Dialogue")]
   //ublic DialogueLine[] dialogueLines;

    #region PRIVATE VARIABLES
    private int currentIndex = 0;

    private Coroutine typingCoroutine;
    private bool justStarted = false;
    private string fullText = "";
    #endregion

    private void OnEnable()
    {
        actionReference.action.Enable();
    }

    private void OnDisable()
    {
        actionReference.action.Disable();
    }

    void Update()
    {
        if (player_detection && actionReference != null && actionReference.action.triggered)
        {

           canva.SetActive(true);
           dialogueUI.SetActive(true);

            Debug.Log("Dialogue Start!");
            if (currentDialogue != null)
            {
                NewDialogue(currentDialogue[currentIndex].dialogueText);
            }
            else
            {
                NewDialogue("");
            }
        }
    }

    void NewDialogue(string text)
    {
        if (dialogueUI != null)
        {
            TextMeshProUGUI tmpText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                if (typingCoroutine != null) 
                {
                    StopCoroutine(typingCoroutine);
                }

                fullText = text;
                typingCoroutine = StartCoroutine(TypeText(tmpText));
            }
        }
    }
    IEnumerator TypeText(TextMeshProUGUI tmpText)
    {
        isTyping = true;
        dialogueFinished = false;
        tmpText.text = "";

        foreach (char letter in fullText.ToCharArray())
        {
            tmpText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        dialogueFinished = true;
    }

private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player_detection = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            player_detection = false;

            if (dialogueUI != null) dialogueUI.SetActive(false);
            if (canva != null) canva.SetActive(false);
            print("Player out of range");
        }
    }
}
