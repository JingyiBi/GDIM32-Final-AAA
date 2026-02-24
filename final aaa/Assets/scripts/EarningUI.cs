using UnityEngine;
using TMPro; 

public class EarningsUI : MonoBehaviour
{
    public TMP_Text currentEarningsText; 
    public TMP_Text totalEarningsText; 
    private int currentDeliveryEarnings;

    private void Update()
    {
        if (currentEarningsText != null)
            currentEarningsText.text = "Current: " + currentDeliveryEarnings.ToString();
        if (totalEarningsText != null)
            totalEarningsText.text = "Total: " + DeliveryManager.Instance.totalEarnings.ToString();
    }

    public void AddCurrentEarnings(int amount)
    {
        currentDeliveryEarnings += amount;
    }

    public void ResetCurrentEarnings()
    {
        currentDeliveryEarnings = 0;
    }
}