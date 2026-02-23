using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    public float interactionDistance = 3f;
    private KeyCode interactKey = KeyCode.I;
    private KeyCode actionKey = KeyCode.E;
    private Transform player;
    private bool isInRange;
    private bool isOrderAssigned;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        isOrderAssigned = false;
    }

    private void Update()
    {
        CheckInteractionRange();
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            InitiateDialogue();
        }
        if (isInRange && Input.GetKeyDown(actionKey) && !isOrderAssigned)
        {
            AssignOrder();
        }
        if (isInRange && Input.GetKeyDown(actionKey) && isOrderAssigned && DeliveryManager.Instance.currentOrder != null 
        && DeliveryManager.Instance.currentOrder.currentState == OrderState.Delivered)
        {
            SubmitOrder();
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;
    }

    private void InitiateDialogue()
    {
        if (!isOrderAssigned)
        {
            if (DeliveryManager.Instance.burgerOrder.isUnlocked && !DeliveryManager.Instance.firstDeliveryCompleted)
            {
                Debug.Log("Owner: Take the burger order! Press E to accept.");
            }
            else if (DeliveryManager.Instance.pizzaOrder.isUnlocked)
            {
                Debug.Log("Owner: Take the pizza order! Press E to accept.");
            }
        }
        else
        {
            Debug.Log("Owner: Back so soon? Press E to submit the order and get paid!");
        }
    }

    private void AssignOrder()
    {
        if (!DeliveryManager.Instance.firstDeliveryCompleted)
        {
            OrderData order = DeliveryManager.Instance.burgerOrder;

            order.currentState = OrderState.Accepted;
            DeliveryManager.Instance.StartOrder(order);

            OrderUI.Instance.UpdateOrderUI(order);
            isOrderAssigned = true;

            Debug.Log("Burger Order Assigned!");
        }
        else if (DeliveryManager.Instance.pizzaOrder.isUnlocked)
        {
            OrderData order = DeliveryManager.Instance.pizzaOrder;

            order.currentState = OrderState.Accepted;
            DeliveryManager.Instance.StartOrder(order);

            OrderUI.Instance.UpdateOrderUI(order);
            isOrderAssigned = true;

            Debug.Log("Pizza Order Assigned!");
        }
    }

    private void SubmitOrder()
    {
        int basePay = DeliveryManager.Instance.currentOrder.basePay;
        DeliveryManager.Instance.AddEarnings(basePay);
        FindObjectOfType<EarningsUI>().AddCurrentEarnings(basePay);
        FindObjectOfType<EarningsUI>().ResetCurrentEarnings();
        
        if (!DeliveryManager.Instance.firstDeliveryCompleted)
        {
            DeliveryManager.Instance.CompleteFirstDelivery();
        }
        DeliveryManager.Instance.currentOrder.currentState = OrderState.Submitted;

        isOrderAssigned = false;
        DeliveryManager.Instance.currentOrder = null;
        OrderUI.Instance.orderPopup.SetActive(false);
        Debug.Log("Order Submitted! Base Pay: " + basePay);
    }
}