using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "Order/OrderData")]
public class OrderData : ScriptableObject
{
    [Header("Customer")]
    public string customerName;

    [Header("Order Info")]
    public Sprite orderImage;
    public string foodType;

    [Header("Payment")]
    public int basePay;

    [Header("Unlock")]
    public bool isUnlocked;

    [Header("Order State")]
    public OrderState currentState = OrderState.Unaccepted;

    [Header("Bonus")]
    public bool cookieOffered;
    public bool cookieCollected;

    public bool IsAccepted()
    {
        return currentState >= OrderState.Accepted;
    }

    public bool IsDelivered()
    {
        return currentState >= OrderState.Delivered;
    }
}

public enum OrderState
{
    Unaccepted,
    Accepted,
    PickedUp,
    Delivered,
    Submitted
}