using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIManager : MonoBehaviour
{
    [Header("Main UI Root")]
    public GameObject startUI;

    [Header("Panels")]
    public GameObject backgroundPanel;
    public GameObject controlPanel;
    public GameObject mainMenuPanel;

    [Header("Buttons")]
    public Button backgroundContinueButton;
    public Button controlContinueButton;
    public Button startGameButton;

    [Header("Text")]
    public TextMeshProUGUI gameTitleText;
    public TextMeshProUGUI teamInfoText;

    void Start()
    {
        Time.timeScale = 0f;

        if (gameTitleText != null)
            gameTitleText.text = "Order Up!";

        if (teamInfoText != null)
            teamInfoText.text = "Team AAA (Jingyi Bi, Peiyi Xiong, Ruixuan Pan)";

        backgroundContinueButton.onClick.AddListener(ShowControlPanel);
        controlContinueButton.onClick.AddListener(ShowMainMenu);
        startGameButton.onClick.AddListener(StartGame);

        ShowBackgroundPanel();
    }

    void ShowBackgroundPanel()
    {
        backgroundPanel.SetActive(true);
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    void ShowControlPanel()
    {
        backgroundPanel.SetActive(false);
        controlPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    void ShowMainMenu()
    {
        backgroundPanel.SetActive(false);
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void StartGame()
    {
        if (startUI != null)
        {
            startUI.SetActive(false);
        }

        Time.timeScale = 1f;

        Debug.Log("Game Started!");
    }
}