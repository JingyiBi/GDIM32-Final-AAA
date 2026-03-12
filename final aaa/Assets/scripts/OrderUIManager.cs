using UnityEngine;
using UnityEngine.UI;

public class OrderUIManager : MonoBehaviour
{
    public Button orderIconButton;
    public GameObject orderPopupPanel;

    public void Start()
    {
        if (orderPopupPanel != null)
            orderPopupPanel.SetActive(false);

        if (orderIconButton != null)
            orderIconButton.interactable = false;
    }

    public void EnableOrderButton()
    {
        if (orderIconButton != null)
            orderIconButton.interactable = true;
        Debug.Log("Order button enabled!");
    }

    public void ShowOrder()
    {
        if (orderPopupPanel != null)
            orderPopupPanel.SetActive(true);
    }

    public void CloseOrder()
    {
        if (orderPopupPanel != null)
            orderPopupPanel.SetActive(false);
    }
}