using UnityEngine;
using UnityEngine.UI;

public class OrderPopupUI : MonoBehaviour
{
    public static OrderPopupUI Instance;

    public GameObject popupPanel;
    public Image popupImage;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);
    }
    public void ShowCurrentOrder()
    {
        if (DeliveryManager.Instance.currentOrder == null) return;
        if (DeliveryManager.Instance.currentOrder.currentState != OrderState.Accepted)
            return;

        popupImage.sprite = DeliveryManager.Instance.currentOrder.orderImage;
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}