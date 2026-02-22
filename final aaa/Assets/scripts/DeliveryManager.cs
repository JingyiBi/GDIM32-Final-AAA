using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;
    public OrderData currentOrder;
    public OrderData burgerOrder;
    public OrderData pizzaOrder;
    public int totalEarnings;
    public bool firstDeliveryCompleted;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        burgerOrder.isUnlocked = true;
        pizzaOrder.isUnlocked = false;
        firstDeliveryCompleted = false;
        totalEarnings = 0;
    }

    public void StartOrder(OrderData order)
    {
        currentOrder = order;
    }

    public void CompleteFirstDelivery()
    {
        firstDeliveryCompleted = true;
        pizzaOrder.isUnlocked = true;
    }

    public void AddEarnings(int amount)
    {
        totalEarnings += amount;
    }
}