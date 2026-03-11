using UnityEngine;
using TMPro;

public class EarningsUI : MonoBehaviour
{
    public TextMeshProUGUI earningsText;

    private void Start()
    {
        
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnOrderSubmitted += UpdateDisplay;
        }
        UpdateDisplay(0);
    }

    private void UpdateDisplay(int amount)
    {
        if (earningsText != null)
            earningsText.text = "Earnings: $" + DeliveryManager.Instance.totalEarnings;
    }

    private void OnDestroy()
    {
        if (OrderManager.Instance != null)
            OrderManager.Instance.OnOrderSubmitted -= UpdateDisplay;
    }
}