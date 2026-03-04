using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;

    private KeyCode interactKey = KeyCode.I;

    private Transform player;
    private bool isInRange;
    public bool isOrderAssigned;

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
        DialogueNode repeatNode = new DialogueNode();
        
        DialogueNode fifthNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "Good luck! Go deliver the meal!",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Got it",
                    nextNode = new DialogueNode
                    {
                        speakerName = "Owner",
                        dialogueText = "",
                        endsDialogue = true
                    }
                },
                new DialogueChoice
                {
                    choiceText = "Please repeat what you just said",
                    nextNode = repeatNode 
                }
            }
        };

        DialogueNode fourthNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "Click on the burger to pick it up.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = fifthNode
                }
            }
        };

        DialogueNode thirdNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "The customer's name and address are in the top right corner, click to enlarge.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = fourthNode
                }
            }
        };

        DialogueNode secondNode = new DialogueNode
        {
            speakerName = "Owner",
            dialogueText = "I will assign you an order.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = thirdNode
                }
            }
        };

        repeatNode.speakerName = "Owner";
        repeatNode.dialogueText = "Hello, you're here!";
        repeatNode.endsDialogue = false;
        repeatNode.autoContinue = false;
        repeatNode.choices = new DialogueChoice[]
        {
            new DialogueChoice
            {
                choiceText = "Continue",
                nextNode = secondNode
            }
        };
        return repeatNode;
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