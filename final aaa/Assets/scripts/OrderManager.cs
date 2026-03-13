using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    public OrderData currentOrder;

    public event System.Action<int> OnOrderSubmitted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AcceptOrder(OrderData order)
    {
        if (order == null) return;

        currentOrder = order;
        currentOrder.currentState = OrderState.Accepted;

        if (GameProgress.Instance != null)
        {
            if (order.foodType == "Burger")
                GameProgress.Instance.firstOrderAccepted = true;

            if (order.foodType == "Pizza")
                GameProgress.Instance.secondOrderAccepted = true;
        }

        OrderUIManager uiManager = FindObjectOfType<OrderUIManager>();
        if (uiManager != null)
            uiManager.EnableOrderButton();

        if (OrderUI.Instance != null)
            OrderUI.Instance.UpdateOrderUI(order);

        Debug.Log("Order accepted: " + order.foodType);
    }

    public void PickUpOrder()
    {
        if (currentOrder == null) return;

        currentOrder.currentState = OrderState.PickedUp;

        Debug.Log("Order picked up: " + currentOrder.foodType);
    }

    public void SubmitOrder()
    {
        if (currentOrder == null) return;

        currentOrder.currentState = OrderState.Submitted;

        OnOrderSubmitted?.Invoke(currentOrder.basePay);

        if (OrderUI.Instance != null)
            OrderUI.Instance.HideOrderUI();

        Debug.Log("Order submitted: " + currentOrder.foodType);
    }
}