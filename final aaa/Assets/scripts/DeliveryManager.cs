using UnityEngine;

public enum GameState { FirstOrder, Transition, SecondOrder, Finished }

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;
    
    [Header("State Machine")]
    public GameState currentGameState = GameState.FirstOrder;

    public OrderData currentOrder;
    public OrderData burgerOrder;
    public OrderData pizzaOrder;
    public int totalEarnings;
    public bool firstDeliveryCompleted;
    public bool secondDeliveryCompleted;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        currentGameState = GameState.FirstOrder;
        if (burgerOrder != null) burgerOrder.isUnlocked = true;
        if (pizzaOrder != null) pizzaOrder.isUnlocked = false;
        firstDeliveryCompleted = false;
        secondDeliveryCompleted = false;
        totalEarnings = 0;
    }

    public void StartOrder(OrderData order)
    {
        currentOrder = order;
        if (OrderUI.Instance != null) OrderUI.Instance.UpdateOrderUI(order);
    }

    public void CompleteFirstDelivery()
    {
        firstDeliveryCompleted = true;
        currentGameState = GameState.Transition;
        if (pizzaOrder != null) pizzaOrder.isUnlocked = true;
    }

    public void StartPizzaPhase()
    {
        currentGameState = GameState.SecondOrder;
        StartOrder(pizzaOrder);
    }

    public void AddEarnings(int amount)
    {
        totalEarnings += amount;
    }
}