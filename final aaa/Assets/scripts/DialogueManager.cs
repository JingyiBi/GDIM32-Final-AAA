using UnityEngine;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueUI dialogueUI;

    private DialogueNode currentNode;

    private Coroutine autoContinueRoutine;

    public event Action OnDialogueEnd;

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
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

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

        GoToNextNode();
    }


    public void ChooseOption(int index)
    {
        if (currentNode == null)
            return;

        if (currentNode.choices == null || index >= currentNode.choices.Length)
            return;

        var choice = currentNode.choices[index];

        if (choice.nextNode == null)
        {
            EndDialogue();
            return;
        }

        currentNode = choice.nextNode;

        DisplayNode();
    }

    void GoToNextNode()
    {
        if (currentNode.endsDialogue)
        {
            EndDialogue();
            return;
        }

        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            DisplayNode();
            return;
        }

        if (currentNode.choices != null && currentNode.choices.Length > 0)
        {
            return; 
        }

        EndDialogue();
    }

    public void EndDialogue()
    {
        dialogueUI.Hide();

        OnDialogueEnd?.Invoke();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}