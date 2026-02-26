using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueUI dialogueUI;

    private DialogueNode currentNode;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(DialogueNode startNode)
    {
        currentNode = startNode;
        dialogueUI.Show();
        DisplayNode();
    }

    void DisplayNode()
    {
        dialogueUI.SetSpeaker(currentNode.speakerName);
        dialogueUI.SetText(currentNode.dialogueText);
        dialogueUI.SetChoices(currentNode.choices);
    }

    public void ChooseOption(int index)
    {
        if (currentNode.choices.Length == 0)
        {
            EndDialogue();
            return;
        }

        currentNode = currentNode.choices[index].nextNode;

        if (currentNode.endsDialogue)
        {
            EndDialogue();
            return;
        }

        DisplayNode();
    }

    public void EndDialogue()
    {
        dialogueUI.Hide();

        RestaurantOwnerNPC owner = FindObjectOfType<RestaurantOwnerNPC>();
        if (owner != null)
        {
            owner.AssignOrder();
        }
    }
}