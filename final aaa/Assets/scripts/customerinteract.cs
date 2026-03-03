using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;
    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool isOrderDelivered = false;
    private bool isDialogueCompleted = false;
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

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        DialogueManager.Instance.OnDialogueEnd += OnDialogueCompletelyFinished;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueCompletelyFinished;
        }
    }

    private void Update()
    {
        if (isOrderDelivered) return;

        CheckInteractionRange();

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            StartCustomerDialogue();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject 
                    && hamburgerInteract != null 
                    && restaurantOwner != null 
                    && restaurantOwner.isOrderAssigned 
                    && isDialogueCompleted)
                {
                    RemoveHamburgerUI();
                    restaurantOwner.SubmitOrder();
                    isOrderDelivered = true;
                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(false);
                }
            }
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange && !isOrderDelivered);
    }

    private void StartCustomerDialogue()
    {
        DialogueManager.Instance.StartDialogue(startNode);
        hamburgerInteract.hasTalkedToCustomer = true;
    }

    private void OnDialogueCompletelyFinished()
    {
        isDialogueCompleted = true;
    }

    private DialogueNode BuildDialogueTree()
    {
        DialogueNode thirdNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Go to the boss to claim your payment.",
            endsDialogue = true,
            autoContinue = false
        };

        DialogueNode secondNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Click on me and your meal will be delivered.",
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

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you for the delivery.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = secondNode
                }
            }
        };

        return firstNode;
    }

    private void RemoveHamburgerUI()
    {
        if (inventoryUIContainer == null) return;
        
        Transform hamburgerIcon = inventoryUIContainer.Find("HamburgerIcon");
        if (hamburgerIcon != null)
        {
            Destroy(hamburgerIcon.gameObject);
        }
        if (hamburgerInteract != null)
        {
            hamburgerInteract.RemoveHamburgerIcon();
        }
    }
}