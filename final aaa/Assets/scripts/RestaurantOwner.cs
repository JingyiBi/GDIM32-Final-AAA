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
        if (isInRange && Input.GetKeyDown(actionKey) && isOrderAssigned && DeliveryManager.Instance.currentOrder != null)
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
            DeliveryManager.Instance.StartOrder(DeliveryManager.Instance.burgerOrder);
            OrderUI.Instance.UpdateOrderUI(DeliveryManager.Instance.burgerOrder);
            isOrderAssigned = true;
            Debug.Log("Burger Order Assigned! Press O to view order.");
        }
        else if (DeliveryManager.Instance.pizzaOrder.isUnlocked)
        {
            DeliveryManager.Instance.StartOrder(DeliveryManager.Instance.pizzaOrder);
            OrderUI.Instance.UpdateOrderUI(DeliveryManager.Instance.pizzaOrder);
            isOrderAssigned = true;
            Debug.Log("Pizza Order Assigned! Press O to view order.");
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

        isOrderAssigned = false;
        DeliveryManager.Instance.currentOrder = null;
        OrderUI.Instance.orderPopup.SetActive(false);
        Debug.Log("Order Submitted! Base Pay: " + basePay);
    }
}