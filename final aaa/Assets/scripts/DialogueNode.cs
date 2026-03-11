using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string speakerName;
    [TextArea(3, 5)] public string dialogueText;
    public DialogueChoice[] choices;
    public bool endsDialogue;
    public DialogueNode nextNode;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
}