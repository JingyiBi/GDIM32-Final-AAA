using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "Order/OrderData")]
public class OrderData : ScriptableObject
{
    public string customerName;
    public Sprite orderImage;
    public string foodType;
    public int basePay;
    public bool isUnlocked;

    public OrderState currentState;
    public bool isCookieOffered;
    public bool isCookiePicked;
}

public enum OrderState
{
    Unaccepted,
    Accepted,
    PickedUp,
    Delivered,
    Submitted
}