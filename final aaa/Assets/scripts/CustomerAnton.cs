using UnityEngine;

public class CustomerAnton : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode nothingNode;
    public DialogueNode burgerNode;
    public DialogueNode pizzaNode;

    [Header("Dependencies")]
    public HamburgerInteract hamburgerItem;
    public PizzaInteract pizzaItem;

    [Header("Interaction Settings")] 
    public float sightAngleThreshold = 0.7f; 
    public float promptDisplayDistance = 5f; 
    public float interactTriggerDistance = 8f; 
    private Transform player;
    private bool isInRange;

    private bool deliveryDialoguePlayed = false;
    public bool orderDelivered = false;
    private bool waitingForClickDelivery = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (player == null || interactionPrompt == null || orderDelivered) 
        {
            interactionPrompt?.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAt = dotProduct > sightAngleThreshold;
        
        Vector3 rayOrigin = player.position + Vector3.up;
        int ignoreLayers = LayerMask.GetMask("Player", "NPC", "Interactable");
        Ray ray = new Ray(rayOrigin, directionToNPC);
        bool hasObstacle = Physics.Raycast(ray, distance, ~ignoreLayers);
        
        isInRange = distance <= promptDisplayDistance && isLookingAt && !hasObstacle && !DialogueManager.Instance.IsDialogueActive;
        interactionPrompt.SetActive(isInRange);
        bool canInteractByKey = distance <= interactTriggerDistance && !DialogueManager.Instance.IsDialogueActive;
        if (canInteractByKey && Input.GetKeyDown(KeyCode.I))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        Debug.Log("Anton interact triggered");
        if (orderDelivered)
        {
            DialogueManager.Instance.StartDialogue(nothingNode);
            return;
        }

        if (hamburgerItem != null && hamburgerItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(burgerNode);
            deliveryDialoguePlayed = true;
            waitingForClickDelivery = true;
            return;
        }
        if (pizzaItem != null && pizzaItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(pizzaNode);
            return;
        }

        DialogueManager.Instance.StartDialogue(nothingNode);
    }

    void OnMouseDown()
    {
        if (!waitingForClickDelivery) return;
        if (!GameProgress.Instance.burgerPickedUp) return;

        if (!DialogueManager.Instance.IsDialogueActive)
        {
            CompleteDelivery();
            waitingForClickDelivery = false;
        }
    }
    void CompleteDelivery()
    {
        OrderManager.Instance.SubmitOrder();
        DeliveryManager.Instance.CompleteFirstDelivery();

        if (hamburgerItem != null)
            hamburgerItem.RemoveFromInventory();

        GameProgress.Instance.burgerPickedUp = false;
        GameProgress.Instance.firstDeliveryCompleted = true;

        orderDelivered = true;
        deliveryDialoguePlayed = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        Debug.Log("Burger delivered!");
    }
}