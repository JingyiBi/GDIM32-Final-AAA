using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    public float interactionDistance = 5f;

    private KeyCode interactKey = KeyCode.I;
    private KeyCode actionKey = KeyCode.E;

    private Transform player;
    private bool isInRange;
    private bool isOrderAssigned;
    private bool isDialogueOpen;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player = playerObj.transform;

        isOrderAssigned = false;
        isDialogueOpen = false;
    }

    private void Update()
    {
        CheckInteractionRange();

        if (!isInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            isDialogueOpen = true;
            InitiateDialogue();
        }

        if (isDialogueOpen && Input.GetKeyDown(actionKey))
        {
            if (!isOrderAssigned)
            {
                AssignOrder();
            }
            else if (DeliveryManager.Instance.currentOrder != null &&
                     DeliveryManager.Instance.currentOrder.currentState == OrderState.Delivered)
            {
                SubmitOrder();
            }

            isDialogueOpen = false;
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
            DialogueUI.Instance.ShowDialogue(
                "Owner: Take the burger order! Press E to accept."
            );
        }
        else
        {
            DialogueUI.Instance.ShowDialogue(
                "Owner: Back so soon? Press E to submit the order!"
            );
        }
    }
    private void AssignOrder()
    {
        if (DeliveryManager.Instance == null)
        {
            Debug.LogError("DeliveryManager is NULL");
            return;
        }

        if (OrderUI.Instance == null)
        {
            Debug.LogError("OrderUI is NULL");
            return;
        }

        OrderData order;

        if (!DeliveryManager.Instance.firstDeliveryCompleted)
        {
            order = DeliveryManager.Instance.burgerOrder;
        }
        else
        {
            order = DeliveryManager.Instance.pizzaOrder;
        }

        if (order == null)
        {
            Debug.LogError("Order is NULL");
            return;
        }

        order.currentState = OrderState.Accepted;
        DeliveryManager.Instance.StartOrder(order);

        OrderUI.Instance.UpdateOrderUI(order);

        isOrderAssigned = true;

        Debug.Log("Order Assigned!");
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