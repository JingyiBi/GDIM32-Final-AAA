using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance;
    public GameObject orderPopup;
    public Image orderImageDisplay;
    public TMP_Text customerNameText;
    public TMP_Text foodTypeText;
    public TMP_Text basePayText;
    private KeyCode orderKey = KeyCode.O;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        orderPopup.SetActive(false);
        UpdateOrderUI(DeliveryManager.Instance.burgerOrder);
    }

    private void Update()
    {
        if (Input.GetKeyDown(orderKey))
        {
            ToggleOrderPopup();
        }
    }

    public void ToggleOrderPopup()
    {
        orderPopup.SetActive(!orderPopup.activeSelf);
        if (orderPopup.activeSelf && DeliveryManager.Instance.currentOrder != null)
        {
            UpdateOrderUI(DeliveryManager.Instance.currentOrder);
        }
    }

    public void UpdateOrderUI(OrderData order)
    {
        if (order == null) return;
        orderImageDisplay.sprite = order.orderImage;
        customerNameText.text = order.customerName;
        foodTypeText.text = order.foodType;
        basePayText.text = order.basePay.ToString();
    }
}