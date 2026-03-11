using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIManager : MonoBehaviour
{
    public static StartUIManager Instance;

    [Header("Main UI")]
    public TextMeshProUGUI gameTitleText;
    public TextMeshProUGUI teamInfoText;
    public Button startGameButton;
    public TextMeshProUGUI startButtonText;
    public GameObject startUIPanel;

    [Header("Guide Panels")]
    public GameObject backgroundPanel;
    public GameObject controlPanel;

    [Header("Guide Buttons")]
    public Button backgroundButton;
    public Button controlButton;

    public bool IsGameStarted { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitTMPText();

        if (startGameButton != null)
            startGameButton.onClick.AddListener(OnStartGameClicked);

        if (backgroundButton != null)
            backgroundButton.onClick.AddListener(ShowBackgroundGuide);

        if (controlButton != null)
            controlButton.onClick.AddListener(ShowControlGuide);

        if (startUIPanel != null)
            startUIPanel.SetActive(true);

        if (backgroundPanel != null)
            backgroundPanel.SetActive(false);

        if (controlPanel != null)
            controlPanel.SetActive(false);

        IsGameStarted = false;
    }

    private void InitTMPText()
    {
        if (gameTitleText != null)
        {
            gameTitleText.text = "Order Up!";
        }

        if (teamInfoText != null)
        {
            teamInfoText.text = "Made by Team AAA (Jingyi Bi, Peiyi Xiong, Ruixuan Pan)";
        }

        if (startButtonText != null)
        {
            startButtonText.text = "Start Game";
        }
    }

    private void OnStartGameClicked()
    {
        if (startUIPanel != null)
            startUIPanel.SetActive(false);

        IsGameStarted = true;

        Debug.Log("Game started! Player can interact");
    }

    private void ShowBackgroundGuide()
    {
        if (backgroundPanel != null)
            backgroundPanel.SetActive(true);

        if (controlPanel != null)
            controlPanel.SetActive(false);
    }

    private void ShowControlGuide()
    {
        if (controlPanel != null)
            controlPanel.SetActive(true);

        if (backgroundPanel != null)
            backgroundPanel.SetActive(false);
    }
}