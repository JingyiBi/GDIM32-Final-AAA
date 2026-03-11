using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance { get; private set; }

    public GameObject orderPanel;
    public TextMeshProUGUI foodTypeText;
    public TextMeshProUGUI payText;
    public Image foodImage;

    private void Awake()
    {
        Instance = this;
        HideOrderUI();
    }

    public void UpdateOrderUI(OrderData order)
    {
        if (order == null) return;
        orderPanel.SetActive(true);
        foodTypeText.text = "Order: " + order.foodType;
        payText.text = "Reward: $" + order.basePay;
        if (foodImage != null) foodImage.sprite = order.orderImage;
    }

    public void HideOrderUI()
    {
        if (orderPanel != null) orderPanel.SetActive(false);
    }
}