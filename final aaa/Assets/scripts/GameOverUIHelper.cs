using UnityEngine;

public class GameOverUIHelper : MonoBehaviour
{
    [Header("UI")]
    public GameObject endPanel;

    private bool hasShown = false;

    void Start()
    {
        if (endPanel != null)
            endPanel.SetActive(false);
    }

    void Update()
    {
        if (hasShown || GameProgress.Instance == null || DialogueManager.Instance.IsDialogueActive)
            return;

        if (GameProgress.Instance.secondOrderRewardClaimed)
        {
            Invoke("ShowGameOverUI", 1f);
            hasShown = true;
        }
    }

    void ShowGameOverUI()
    {
        if (endPanel != null)
            endPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}