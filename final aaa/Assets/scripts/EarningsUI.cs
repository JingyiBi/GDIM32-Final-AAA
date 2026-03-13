using UnityEngine;
using TMPro;

public class EarningsUI : MonoBehaviour
{
    public TextMeshProUGUI earningsText;

    private void Start()
    {
        RefreshDisplay();
    }
    void Update()
    {
        earningsText.text = "Total Earnings: $" + DeliveryManager.Instance.totalEarnings;
    }
    public void RefreshDisplay()
    {
        if (earningsText != null && DeliveryManager.Instance != null)
        {
            earningsText.text = "Total Earning: $" + DeliveryManager.Instance.totalEarnings;
        }
    }
}