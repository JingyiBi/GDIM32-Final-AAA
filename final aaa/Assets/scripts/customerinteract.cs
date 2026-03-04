using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;
    public EarningsUI earningsUI;
    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool isOrderDelivered = false;
    private bool isDialogueCompleted = false;
    private DialogueNode startNode;
    private bool isRewardGiven = false;
    private bool isDialogueStarted = false;

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
            isDialogueStarted = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!restaurantOwner.isOrderAssigned)
                    {
                        Debug.Log("You don't have an order yet.");
                        return;
                    }

                    if (!isDialogueStarted)
                    {
                        Debug.Log("Talk to Anton first.");
                        return;
                    }

                    if (!isDialogueCompleted)
                    {
                        Debug.Log("Finish the conversation before delivering.");
                        return;
                    }

                    RemoveHamburgerUI();
                    restaurantOwner.SubmitOrder();
                    isOrderDelivered = true;

                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(false);

                    Debug.Log("Burger delivered successfully!");
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
        if (!isRewardGiven && earningsUI != null)
        {
            earningsUI.ResetCurrentEarnings(); 
            earningsUI.AddCurrentEarnings(50); 
            isRewardGiven = true;
        }
    }

    private DialogueNode BuildDialogueTree()
    {
        DialogueNode fourthNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "oh",
            endsDialogue = true,
            autoContinue = false
        };


        

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you for the delivery. Click on me and your meal will be delivered. I will give you 50 dollars.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "OK",
                    nextNode = fourthNode
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