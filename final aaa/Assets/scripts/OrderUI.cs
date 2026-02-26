using UnityEngine;
using TMPro;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance;

    [Header("UI References")]
    public GameObject orderPanel;  
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI payText;
    public TextMeshProUGUI statusText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (orderPanel != null)
            orderPanel.SetActive(false);
    }

    public void UpdateOrderUI(OrderData order)
    {
        if (order == null) return;

        orderPanel.SetActive(true);

        titleText.text = "Current Order";
        itemText.text = "Food: " + order.foodType;
        payText.text = "Pay: $" + order.basePay;
        statusText.text = "Status: " + order.currentState.ToString();
    }

    public void HideOrderUI()
    {
        if (orderPanel != null)
            orderPanel.SetActive(false);
    }
}