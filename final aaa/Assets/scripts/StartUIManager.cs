using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIManager : MonoBehaviour
{
    public static StartUIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI gameTitleText;
    public TextMeshProUGUI teamInfoText;
    public Button startGameButton;
    public TextMeshProUGUI startButtonText;
    public GameObject startUIPanel;

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
        {
            startGameButton.onClick.AddListener(OnStartGameClicked);
        }

        if (startUIPanel != null)
        {
            startUIPanel.SetActive(true);
        }

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
        {
            startUIPanel.SetActive(false);
        }

        IsGameStarted = true;

        Debug.Log("Game started! Player can interact");
    }
}