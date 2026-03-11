using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public DialogueUI dialogueUI;
    public event Action OnDialogueEnd;
    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartDialogue(DialogueNode node)
    {
        if (node == null) return;
        IsDialogueActive = true;
        dialogueUI.Show();
        DisplayNode(node);
    }

    private void DisplayNode(DialogueNode node)
    {
        dialogueUI.SetSpeaker(node.speakerName);
        dialogueUI.SetText(node.dialogueText);
        dialogueUI.SetChoices(node.choices, (nextNode) => 
        {
            if (nextNode != null) DisplayNode(nextNode);
            else EndDialogue();
        });

        if (node.endsDialogue && (node.choices == null || node.choices.Length == 0))
        {
            
        }
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueUI.Hide();
        OnDialogueEnd?.Invoke();
    }
}
