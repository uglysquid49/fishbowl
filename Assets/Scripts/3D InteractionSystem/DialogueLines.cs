using UnityEngine;
public enum SpeakerType
{
    Items
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(3, 10)]
    public string dialogueText;
    public string interactionSoundName;
}
