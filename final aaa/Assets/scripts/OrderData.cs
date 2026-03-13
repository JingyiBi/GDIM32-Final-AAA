using UnityEngine;

public enum OrderState { Unaccepted, Accepted, PickedUp, Delivered, Submitted }

[CreateAssetMenu(fileName = "NewOrder", menuName = "Order/OrderData")]
public class OrderData : ScriptableObject
{
    public string customerName;
    public Sprite orderImage;
    public string foodType;
    public int basePay;
    public int tipPay;
    public bool isUnlocked;
    public OrderState currentState = OrderState.Unaccepted;
}
