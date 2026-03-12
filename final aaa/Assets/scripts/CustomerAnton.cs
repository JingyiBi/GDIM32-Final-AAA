using UnityEngine;

public class CustomerAnton : InteractableBase
{
    public DialogueNode nothingNode;
    public DialogueNode burgerNode;
    public DialogueNode pizzaNode;

    public HamburgerInteract hamburgerItem;
    public PizzaInteract pizzaItem;

    [Header("Interaction Settings")]
    public float sightAngleThreshold = 0.5f;

    private Transform player;
    private bool isInRange;

    private bool deliveryDialoguePlayed = false;
    private bool orderDelivered = false;
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
        if (player == null || interactionPrompt == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inDistance = distance <= interactionDistance;

        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAt = dotProduct > sightAngleThreshold;

        bool noObstacle = true;
        if (Physics.Raycast(player.position, directionToNPC, out RaycastHit hit, interactionDistance))
        {
            noObstacle = hit.collider.gameObject == gameObject;
        }

        isInRange = inDistance && isLookingAt && noObstacle;

        interactionPrompt.SetActive(isInRange);

        if (isInRange && Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.IsDialogueActive)
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

        Debug.Log("Burger delivered!");
    }
}