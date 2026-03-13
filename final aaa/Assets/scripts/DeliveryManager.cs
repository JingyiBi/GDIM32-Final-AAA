using UnityEngine;

public enum GameState { FirstOrder, Transition, SecondOrder, Finished }

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [Header("State Machine")]
    public GameState currentGameState = GameState.FirstOrder;

    public OrderData burgerOrder;
    public OrderData pizzaOrder;
    public int totalEarnings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEarnings(int amount)
    {
        totalEarnings += amount;
    }

    public void CompleteFirstDelivery()
    {
        currentGameState = GameState.Transition;

        if (pizzaOrder != null)
            pizzaOrder.isUnlocked = true;
    }

    public void StartPizzaPhase()
    {
        currentGameState = GameState.SecondOrder;
        OrderManager.Instance.AcceptOrder(pizzaOrder);
    }

    public void CompleteSecondDelivery()
    {
        Debug.Log("Second delivery completed!");

        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.secondDeliveryCompleted = true;
        }

        currentGameState = GameState.Finished;  
    }
}