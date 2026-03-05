using UnityEngine;
using TMPro;

public class FloatingHintManager : MonoBehaviour
{
    public static FloatingHintManager Instance;
    public GameObject hintPanel;
    public TextMeshProUGUI hintText;

    private void Awake()
    {
        Instance = this;
        if (hintPanel != null) hintPanel.SetActive(false);
    }

    public void ShowHint(string message)
    {
        if (hintPanel != null)
        {
            hintText.text = message;
            hintPanel.SetActive(true);
            CancelInvoke("HideHint");
            Invoke("HideHint", 5f);
        }
    }

    private void HideHint()
    {
        if (hintPanel != null) hintPanel.SetActive(false);
    }
}