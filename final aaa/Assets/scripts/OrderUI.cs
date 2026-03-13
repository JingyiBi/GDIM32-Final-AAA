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

    [Header("Popup UI")]
    public GameObject orderPopupPanel;
    public Image popupOrderImage;

    private OrderData currentOrder;
    private void Awake()
    {
        Instance = this;
        HideOrderUI();

        if (orderPopupPanel != null)
            orderPopupPanel.SetActive(false);
    }

    public void UpdateOrderUI(OrderData order)
    {
        if (order == null) return;

        currentOrder = order;

        orderPanel.SetActive(true);
        foodTypeText.text = "Order: " + order.foodType;
        payText.text = "Reward: $" + order.basePay;

        if (foodImage != null)
            foodImage.sprite = order.orderImage;
    }

    public void HideOrderUI()
    {
        if (orderPanel != null) orderPanel.SetActive(false);
    }
    public void ShowOrderPopup()
    {
        if (currentOrder == null) return;

        Debug.Log("Popup Order: " + currentOrder.foodType);
        Debug.Log(currentOrder.orderImage.name);

        orderPopupPanel.SetActive(true);
        popupOrderImage.sprite = currentOrder.orderImage;

    }

    public void CloseOrderPopup()
    {
        orderPopupPanel.SetActive(false);
    }
}