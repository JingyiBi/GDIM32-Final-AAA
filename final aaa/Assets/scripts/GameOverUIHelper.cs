using UnityEngine;
using TMPro;

public class GameOverUIHelper : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject endPanel;        
    public TextMeshProUGUI resultText; 

    private bool hasShown = false;

    void Start() 
    {
        if(endPanel != null) endPanel.SetActive(false); 
    }

    void Update()
    {
        if (hasShown || GameProgress.Instance == null) return;

        
        if (GameProgress.Instance.secondOrderRewardClaimed)
        {
            
            Invoke("ShowFinalResult", 1.0f);
            hasShown = true; 
        }
    }

    void ShowFinalResult()
    {
        if(endPanel != null) endPanel.SetActive(true);
        
        int total = DeliveryManager.Instance.totalEarnings;
        if(resultText != null) 
            resultText.text = $" Your delivery today was successfully completed, with a total income of {total}";
            
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}