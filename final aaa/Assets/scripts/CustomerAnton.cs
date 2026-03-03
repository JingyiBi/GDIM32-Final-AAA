using UnityEngine;
using TMPro;

public class CustomerAnton : MonoBehaviour
{
    [Header("References")]
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public GameObject interactionPrompt;       
    public TextMeshProUGUI interactionPromptText; 

    [Header("Settings")]
    public float interactionDistance = 20f;
    public int tipAmount = 20;
    public int fortuneCookieExtraTip = 10;

    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool hasDelivered = false;
    private bool isDialogueCompleted = false;  
    private DialogueNode startNode;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) return;
        player = playerObj.transform;

        startNode = BuildDialogueTree();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd += OnDialogueCompletelyFinished;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueCompletelyFinished;
    }

    private void Update()
    {
        if (player == null || hasDelivered) return;

        CheckInteractionRange();

        if (isInRange && Input.GetKeyDown(interactKey) && !isDialogueCompleted)
        {
            if (DeliveryManager.Instance.currentOrder != null && 
                DeliveryManager.Instance.currentOrder.currentState == OrderState.PickedUp)
            {
                StartCustomerDialogue();
            }
        }

        if (isDialogueCompleted && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    DeliverFood();
                }
            }
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
        {
            bool shouldShow = isInRange && !hasDelivered && !isDialogueCompleted;
            interactionPrompt.SetActive(shouldShow);
            
            if (shouldShow && interactionPromptText != null)
            {
                interactionPromptText.text = "Press 'I' to talk";
            }
        }
    }

    private void StartCustomerDialogue()
    {
        if (DialogueManager.Instance != null && startNode != null)
        {
            DialogueManager.Instance.StartDialogue(startNode);
        }
    }

    private void OnDialogueCompletelyFinished()
    {
        isDialogueCompleted = true;
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void DeliverFood()
    {
        if (DeliveryManager.Instance.currentOrder == null) return;

        bool hasCookie = DeliveryManager.Instance.currentOrder.isCookiePicked;
        int totalTip = hasCookie ? tipAmount + fortuneCookieExtraTip : tipAmount;

        DeliveryManager.Instance.AddEarnings(totalTip);
        
        EarningsUI eUI = FindObjectOfType<EarningsUI>();
        if (eUI != null) eUI.AddCurrentEarnings(totalTip);

        DeliveryManager.Instance.currentOrder.currentState = OrderState.Delivered;
        hasDelivered = true;

        RemoveHamburgerUI();
    }

    private void RemoveHamburgerUI()
    {
        if (inventoryUIContainer != null)
        {
            Transform hamburgerIcon = inventoryUIContainer.Find("HamburgerIcon");
            if (hamburgerIcon != null)
                Destroy(hamburgerIcon.gameObject);
        }

        if (hamburgerInteract != null)
            hamburgerInteract.RemoveHamburgerIcon();
    }

    private DialogueNode BuildDialogueTree()
    {
        DialogueNode thirdNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Click on me and I'll take the food!",
            endsDialogue = true,
            autoContinue = false
        };

        DialogueNode secondNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Finally! I've been waiting forever.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice { choiceText = "Here's your order!", nextNode = thirdNode }
            }
        };

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Oh, the delivery is here!",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice { choiceText = "Hi! I have your order.", nextNode = secondNode }
            }
        };

        return firstNode;
    }
}