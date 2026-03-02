using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueUI dialogueUI;

    private DialogueNode currentNode;

    private Coroutine autoContinueRoutine;

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

        if (currentNode.autoContinue)
        {
            if (autoContinueRoutine != null)
                StopCoroutine(autoContinueRoutine);

            autoContinueRoutine = StartCoroutine(AutoContinue());
        }
    }

    IEnumerator AutoContinue()
    {
        yield return new WaitForSeconds(currentNode.autoContinueDelay);

        if (currentNode.endsDialogue)
        {
            EndDialogue();
            yield break;
        }

        if (currentNode.choices != null && currentNode.choices.Length > 0)
        {
            currentNode = currentNode.choices[0].nextNode;
            DisplayNode();
        }
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