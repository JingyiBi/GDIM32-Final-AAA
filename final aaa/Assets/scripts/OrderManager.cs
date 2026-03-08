using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public OrderData currentOrder;

    void Awake()
    {
        Instance = this;
    }

    public void AcceptOrder(OrderData order)
    {
        currentOrder = order;
        currentOrder.currentState = OrderState.Accepted;
    }

    public void PickUpOrder()
    {
        if (currentOrder == null) return;
        currentOrder.currentState = OrderState.PickedUp;
    }

    public void DeliverOrder()
    {
        if (currentOrder == null) return;
        currentOrder.currentState = OrderState.Delivered;
    }

    public void SubmitOrder()
    {
        if (currentOrder == null) return;

        currentOrder.currentState = OrderState.Submitted;

        DeliveryManager.Instance.AddEarnings(currentOrder.basePay);
    }
}