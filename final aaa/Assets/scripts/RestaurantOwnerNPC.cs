using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;

    private KeyCode interactKey = KeyCode.I;

    private Transform player;
    private bool isInRange;
    private bool isOrderAssigned;

    private DialogueNode startNode;   

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player = playerObj.transform;

        startNode = BuildDialogueTree();

        isOrderAssigned = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        CheckInteractionRange();

        if (!isInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartOwnerDialogue();
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    private void StartOwnerDialogue()
    {
        DialogueManager.Instance.StartDialogue(startNode);
    }

    private DialogueNode BuildDialogueTree()
    {
        DialogueNode acceptNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "Great! One Burger. $5 base pay. Don't keep the customer waiting!",
            endsDialogue = true
        };

        DialogueNode payNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "You get $5 base pay per delivery. Treat customers well � they might tip you!",
            endsDialogue = true
        };

        DialogueNode start = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "Hey there! You must be our new delivery rider. Think you can handle an order?",
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Sure, what's the order?",
                    nextNode = acceptNode
                },
                new DialogueChoice
                {
                    choiceText = "What's in it for me?",
                    nextNode = payNode
                }
            }
        };

        return start;
    }

    public void AssignOrder()
    {
        if (DeliveryManager.Instance == null || OrderUI.Instance == null)
        {
            Debug.LogError("Missing Managers");
            return;
        }

        OrderData order;

        if (!DeliveryManager.Instance.firstDeliveryCompleted)
            order = DeliveryManager.Instance.burgerOrder;
        else
            order = DeliveryManager.Instance.pizzaOrder;

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

    public void SubmitOrder()
    {
        if (DeliveryManager.Instance.currentOrder == null) return;

        int basePay = DeliveryManager.Instance.currentOrder.basePay;

        DeliveryManager.Instance.AddEarnings(basePay);

        EarningsUI earnings = FindObjectOfType<EarningsUI>();
        if (earnings != null)
        {
            earnings.AddCurrentEarnings(basePay);
            earnings.ResetCurrentEarnings();
        }

        DeliveryManager.Instance.currentOrder.currentState = OrderState.Submitted;

        isOrderAssigned = false;
        DeliveryManager.Instance.currentOrder = null;

        OrderUI.Instance.HideOrderUI();

        Debug.Log("Order Submitted!");
    }
}