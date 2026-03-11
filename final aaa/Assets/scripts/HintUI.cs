using UnityEngine;
using TMPro;

public class HintUI : MonoBehaviour
{
    public static HintUI Instance { get; private set; }
    public TextMeshProUGUI hintText;
    public GameObject panel;

    private void Awake() => Instance = this;

    public void ShowHint(string message)
    {
        panel.SetActive(true);
        hintText.text = message;
    }

    public void HideHint() => panel.SetActive(false);
}
