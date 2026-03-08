using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Basic Info")]
    public string speakerName;

    [TextArea(3, 5)]
    public string dialogueText;

    [Header("Branching")]
    public DialogueChoice[] choices;

    [Header("Ending / Flow")]
    public bool endsDialogue;

    public bool autoContinue;
    public float autoContinueDelay = 2f;

    public DialogueNode nextNode;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
}