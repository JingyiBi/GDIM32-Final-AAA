using UnityEngine;

public class CustomerAnton : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode anton_Nothing_on_hand;    
    public DialogueNode anton_Burger_on_hand;    
    public DialogueNode anton_Pizza_on_hand;     

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
        
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
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
        orderDelivered = false;
        bool hasBurger = GameProgress.Instance.burgerPickedUp;
        bool hasPizza = GameProgress.Instance.pizzaPickedUp;

        if (!GameProgress.Instance.hasTalkedToOwner)
        {
            DialogueManager.Instance.StartDialogue(anton_Nothing_on_hand);
            return;
        }

        if (hasBurger && !hasPizza)
        {
            DialogueManager.Instance.StartDialogue(anton_Burger_on_hand);
            deliveryDialoguePlayed = true;
            waitingForClickDelivery = true;
            return;
        }

        if (hasPizza && !hasBurger)
        {
            DialogueManager.Instance.StartDialogue(anton_Pizza_on_hand);
            return;
        }
        
        DialogueManager.Instance.StartDialogue(anton_Nothing_on_hand);
    }

    void OnMouseDown()
    {
        if (!waitingForClickDelivery || !GameProgress.Instance.burgerPickedUp || DialogueManager.Instance.IsDialogueActive) return;
        CompleteDelivery();
        waitingForClickDelivery = false;
    }

    void CompleteDelivery()
    {
        OrderManager.Instance.SubmitOrder();
        DeliveryManager.Instance.CompleteFirstDelivery();
        if (hamburgerItem != null) hamburgerItem.RemoveFromInventory();
        GameProgress.Instance.burgerPickedUp = false;
        GameProgress.Instance.firstDeliveryCompleted = true;
        orderDelivered = true;
        deliveryDialoguePlayed = false;
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }
}