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
    }

    void DisplayNode()
    {
        dialogueUI.SetSpeaker(currentNode.speakerName);
        dialogueUI.SetText(currentNode.dialogueText);

        if (currentNode.autoContinue)
            dialogueUI.SetChoices(null);
        else
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

        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            DisplayNode();
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
        if (currentNode == null) return;
        if (currentNode.choices == null) return;
        if (index < 0 || index >= currentNode.choices.Length) return;

        currentNode = currentNode.choices[index].nextNode;

        DisplayNode();  

        if (currentNode.endsDialogue)
        {
            StartCoroutine(EndAfterDelay());
        }
    }

    IEnumerator EndAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        EndDialogue();
    }

    public void EndDialogue()
    {
        dialogueUI.Hide();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        RestaurantOwnerNPC owner = FindObjectOfType<RestaurantOwnerNPC>();
        if (owner != null && !owner.isOrderAssigned)
        {
            owner.AssignOrder();
            Debug.Log("Order assigned from EndDialogue");
        }
    }
}