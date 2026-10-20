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
    //public DialogueLine[] dialogueLines;

    #region PRIVATE VARIABLES
    private int currentIndex = 0;

    private Coroutine typingCoroutine;
    private bool justStarted = false;
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
            Debug.Log("Dialogue Start!");
        }
    }


    void NewDialogue (string text)
    {
        GameObject template_clone = Instantiate(dialogueUI, dialogueUI.transform);
        template_clone.transform.parent = canva.transform;
        template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
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
        player_detection = false;
    }
}
