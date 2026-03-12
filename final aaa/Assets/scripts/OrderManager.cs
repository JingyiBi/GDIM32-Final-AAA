using UnityEngine;
using System.Collections.Generic;   

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    public OrderData currentOrder;

    
    public event System.Action<int> OnOrderSubmitted;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AcceptOrder(OrderData order)
    {
        currentOrder = order;
        currentOrder.currentState = OrderState.Accepted;

        if (GameProgress.Instance != null)
            GameProgress.Instance.firstOrderAccepted = true;

        OrderUIManager ui = FindObjectOfType<OrderUIManager>();
        if (ui != null)
            ui.EnableOrderButton();

        Debug.Log("First order accepted!");
    }

    public void PickUpOrder()
    {
        if (currentOrder != null) currentOrder.currentState = OrderState.PickedUp;
    }

    public void SubmitOrder()
    {
        if (currentOrder == null) return;
        
        currentOrder.currentState = OrderState.Submitted;
        
        OnOrderSubmitted?.Invoke(currentOrder.basePay);
        
        if (OrderUI.Instance != null) OrderUI.Instance.HideOrderUI();
    }
}
