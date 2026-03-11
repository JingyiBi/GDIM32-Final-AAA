using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public Button startButton;

    private void Start()
    {
        Time.timeScale = 0f; 
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
