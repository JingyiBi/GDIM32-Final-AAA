using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI contentText;
    public Button[] choiceButtons;

    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);

    public void SetSpeaker(string name) => speakerText.text = name;
    public void SetText(string text) => contentText.text = text;

    public void SetChoices(DialogueChoice[] choices, Action<DialogueNode> onChoiceSelected)
    {
        foreach (var btn in choiceButtons) btn.gameObject.SetActive(false);

        if (choices == null || choices.Length == 0) return;

        for (int i = 0; i < choices.Length && i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
            
            int index = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => onChoiceSelected(choices[index].nextNode));
        }
    }
}