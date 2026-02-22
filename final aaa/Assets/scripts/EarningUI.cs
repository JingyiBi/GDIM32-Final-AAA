using UnityEngine;
using UnityEngine.UI;

public class EarningsUI : MonoBehaviour
{
    public Text currentEarningsText;
    public Text totalEarningsText;
    private int currentDeliveryEarnings;

    private void Update()
    {
        currentEarningsText.text = currentDeliveryEarnings.ToString();
        totalEarningsText.text = DeliveryManager.Instance.totalEarnings.ToString();
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