using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;

    [System.NonSerialized] 
    public DialogueNode nextNode;
}

[System.Serializable]
public class DialogueNode
{
    public string speakerName;

    [TextArea(3, 5)]
    public string dialogueText;

    public DialogueChoice[] choices;

    public bool endsDialogue;

    public bool autoContinue;
    public float autoContinueDelay = 1.5f;
}