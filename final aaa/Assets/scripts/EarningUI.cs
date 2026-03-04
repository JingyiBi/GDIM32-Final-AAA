using UnityEngine;
using TMPro; 

public class EarningsUI : MonoBehaviour
{
    public TMP_Text totalEarningsText; 
    private int currentDeliveryEarnings;

    private void Start()
    {
        UpdateEarningsDisplay();
    }

    private void Update()
    {
        UpdateEarningsDisplay();
    }

    private void UpdateEarningsDisplay()
    {
        if (totalEarningsText != null)
            totalEarningsText.text = "Total Earning: " + (DeliveryManager.Instance?.totalEarnings.ToString() ?? "0");
    }

    public void AddCurrentEarnings(int amount)
    {
        currentDeliveryEarnings += amount;
        UpdateEarningsDisplay(); 
    }

    public void ResetCurrentEarnings()
    {
        currentDeliveryEarnings = 0;
        UpdateEarningsDisplay(); 
    }
}