using UnityEngine;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public DialogueUI dialogueUI;

    private DialogueNode currentNode;
    private Coroutine autoContinueRoutine;

    public bool IsDialogueActive { get; private set; }

    public event Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI is not assigned in DialogueManager.");
            return;
        }

        if (startNode == null)
        {
            Debug.LogWarning("StartDialogue called with null startNode.");
            return;
        }

        if (autoContinueRoutine != null)
        {
            StopCoroutine(autoContinueRoutine);
            autoContinueRoutine = null;
        }

        currentNode = startNode;
        IsDialogueActive = true;

        dialogueUI.Show();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisplayNode();
    }

    private void DisplayNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        dialogueUI.SetSpeaker(currentNode.speakerName);
        dialogueUI.SetText(currentNode.dialogueText);
        dialogueUI.SetChoices(currentNode.choices);

        bool hasChoices = currentNode.choices != null && currentNode.choices.Length > 0;

        if (hasChoices)
        {
            return;
        }

        if (currentNode.autoContinue)
        {
            if (autoContinueRoutine != null)
            {
                StopCoroutine(autoContinueRoutine);
            }

            autoContinueRoutine = StartCoroutine(AutoContinue(currentNode));
            return;
        }

        if (currentNode.endsDialogue)
        {
            if (autoContinueRoutine != null)
            {
                StopCoroutine(autoContinueRoutine);
            }

            autoContinueRoutine = StartCoroutine(EndAfterDelay(0.15f));
            return;
        }

        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            DisplayNode();
            return;
        }

        EndDialogue();
    }

    private IEnumerator AutoContinue(DialogueNode nodeAtStart)
    {
        yield return new WaitForSeconds(nodeAtStart.autoContinueDelay);

        if (currentNode != nodeAtStart)
        {
            yield break;
        }

        GoToNextNode();
    }

    private IEnumerator EndAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndDialogue();
    }

    public void ChooseOption(int index)
    {
        if (currentNode == null)
            return;

        if (currentNode.choices == null || index < 0 || index >= currentNode.choices.Length)
            return;

        if (autoContinueRoutine != null)
        {
            StopCoroutine(autoContinueRoutine);
            autoContinueRoutine = null;
        }

        DialogueChoice choice = currentNode.choices[index];

        if (choice == null || choice.nextNode == null)
        {
            EndDialogue();
            return;
        }

        currentNode = choice.nextNode;
        DisplayNode();
    }

    private void GoToNextNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

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

        bool hasChoices = currentNode.choices != null && currentNode.choices.Length > 0;
        if (hasChoices)
        {
            return;
        }

        EndDialogue();
    }

    public void EndDialogue()
    {
        if (autoContinueRoutine != null)
        {
            StopCoroutine(autoContinueRoutine);
            autoContinueRoutine = null;
        }

        currentNode = null;
        IsDialogueActive = false;

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        OnDialogueEnd?.Invoke();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}