using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject panel;
    public TextMeshProUGUI dialogueText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowDialogue(string text)
    {
        panel.SetActive(true);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        panel.SetActive(false);
    }
}