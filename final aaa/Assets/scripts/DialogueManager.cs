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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisplayNode();
        Debug.Log(dialogueUI);
    }

    void DisplayNode()
    {
        dialogueUI.SetSpeaker(currentNode.speakerName);
        dialogueUI.SetText(currentNode.dialogueText);
        dialogueUI.SetChoices(currentNode.choices);
    }

    public void ChooseOption(int index)
    {
        Debug.Log($"ChooseOption index={index}");

        if (currentNode == null)
        {
            Debug.LogError("currentNode is NULL");
            return;
        }

        if (currentNode.choices == null)
        {
            Debug.LogError("currentNode.choices is NULL");
            return;
        }

        if (index < 0 || index >= currentNode.choices.Length)
        {
            Debug.LogError($"Index out of range. choices.Length={currentNode.choices.Length}, index={index}");
            return;
        }

        if (currentNode.choices.Length == 0)
        {
            EndDialogue();
            return;
        }

        var chosen = currentNode.choices[index];
        if (chosen == null)
        {
            Debug.LogError($"Choice[{index}] is NULL");
            return;
        }

        if (chosen.nextNode == null)
        {
            Debug.LogError($"Choice[{index}] nextNode is NULL. choiceText={chosen.choiceText}");
            return;
        }

        currentNode = chosen.nextNode;

        if (currentNode.endsDialogue || currentNode.choices == null || currentNode.choices.Length == 0)
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}