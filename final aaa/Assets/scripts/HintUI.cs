using UnityEngine;
using TMPro;

public class HintUI : MonoBehaviour
{
    public static HintUI Instance;

    public GameObject hintPanel;
    public TextMeshProUGUI hintText;

    private void Awake()
    {
        Instance = this;
        hintPanel.SetActive(false);
    }

    public void ShowHint(string message)
    {
        hintText.text = message;
        hintPanel.SetActive(true);
    }

    public void HideHint()
    {
        hintPanel.SetActive(false);
    }
}