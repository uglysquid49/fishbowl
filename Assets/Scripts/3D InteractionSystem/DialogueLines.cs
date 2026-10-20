using UnityEngine;
public enum SpeakerType
{
    Items
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(1, 3)]
    public string dialogueText;
}