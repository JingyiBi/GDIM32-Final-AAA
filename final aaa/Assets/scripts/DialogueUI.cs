using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void SetSpeaker(string name)
    {
        speakerText.text = name;
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }

    public void SetChoices(DialogueChoice[] choices)
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].gameObject.SetActive(false);

 
            if (i < optionTexts.Length && optionTexts[i] != null)
                optionTexts[i].text = "";
        }

        if (choices == null || choices.Length == 0)
            return;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i >= choices.Length) break;

            optionButtons[i].gameObject.SetActive(true);
            if (i < optionTexts.Length && optionTexts[i] != null)
                optionTexts[i].text = choices[i].choiceText ?? "";

            int index = i;
            optionButtons[i].onClick.AddListener(() =>
            {
                DialogueManager.Instance.ChooseOption(index);
            });
        }
    }
}