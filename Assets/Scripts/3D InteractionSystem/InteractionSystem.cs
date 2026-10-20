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
    public TextMeshProUGUI speakerText;
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
    //public DialogueLine[] dialogueLines;

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
            if (isTyping)
            {
                DialogueFinished();
                return;
            }

            if (currentDialogue != null && currentDialogue.Length > 0)
            {
                if (currentIndex < currentDialogue.Length)
                {
                    SoundManager.instance.PlaySound3D("Interaction", transform.position);
                    canva.SetActive(true);
                    dialogueUI.SetActive(true);

                    //sfx for interaction
                    string soundName = currentDialogue[currentIndex].interactionSoundName;
                    if (!string.IsNullOrEmpty(soundName) && SoundManager.instance != null)
                    {
                        SoundManager.instance.PlaySound2D(soundName);
                    print("sound on");
                    }

                    NewDialogue(currentDialogue[currentIndex].speakerName, currentDialogue[currentIndex].dialogueText);

                    currentIndex++;
                    Debug.Log("currently yapping");
                }
                else
                {
                    EndDialogue();
                    Debug.Log("Dialogue over...");
                }
            }
        }
    }

    void NewDialogue(string speaker, string text)
    {
        if (dialogueUI != null)
        {
            TextMeshProUGUI tmpText = textBox != null ? textBox : dialogueUI.GetComponentInChildren<TextMeshProUGUI>();

            if (speakerText != null)
            {
                speakerText.text = speaker;
            }

            if (tmpText != null)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
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
    void DialogueFinished()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        TextMeshProUGUI tmpText = textBox != null ? textBox : dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = fullText;
        }

        isTyping = false;
        dialogueFinished = true;
    }
    void EndDialogue()
    {
        if (dialogueUI != null) dialogueUI.SetActive(false);
        if (canva != null) canva.SetActive(false);

        currentIndex = 0;
        dialogueFinished = true;
        isTyping = false;
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

