using UnityEngine;

public class AmyInteract : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode amy_手上无东西;
    public DialogueNode amy_手上有汉堡时;
    public DialogueNode amy_手上只有pizza;
    public DialogueNode amy_pizza_plus_cookie;

    [Header("Dependencies")]
    public PizzaInteract pizzaItem;
    public CookiePickup cookieItem;

    [Header("Interaction Settings")]
    public float sightAngleThreshold = 0.7f;
    public float promptDisplayDistance = 5f;
    public float interactTriggerDistance = 8f;

    private Transform player;
    private bool isInRange;

    private bool waitingForClickDelivery = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (player == null || interactionPrompt == null)
        {
            interactionPrompt?.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;

        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAt = dotProduct > sightAngleThreshold;

        bool canInteract =
            distance <= promptDisplayDistance &&
            isLookingAt &&
            !DialogueManager.Instance.IsDialogueActive;

        interactionPrompt.SetActive(canInteract);

        if (canInteract && Input.GetKeyDown(KeyCode.I))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (waitingForClickDelivery)
            return;

        bool hasBurger = GameProgress.Instance.burgerPickedUp;
        bool hasPizza = GameProgress.Instance.pizzaPickedUp;
        bool hasCookie = GameProgress.Instance.cookiePickedUp;

        if (!GameProgress.Instance.hasTalkedToOwner)
        {
            DialogueManager.Instance.StartDialogue(amy_手上无东西);
            return;
        }

        if (hasBurger)
        {
            DialogueManager.Instance.StartDialogue(amy_手上有汉堡时);
            return;
        }

        if (hasPizza)
        {
            DialogueManager.Instance.StartDialogue(amy_手上只有pizza);
            waitingForClickDelivery = true;
            return;
        }

        DialogueManager.Instance.StartDialogue(amy_手上无东西);
    }

    private void OnMouseDown()
    {
        if (!waitingForClickDelivery) return;

        if (DialogueManager.Instance.IsDialogueActive) return;

        CompleteDelivery();
        waitingForClickDelivery = false;
    }

    private void CompleteDelivery()
    {
        OrderManager.Instance.SubmitOrder();

        if (pizzaItem != null)
            pizzaItem.RemoveFromInventory();

        GameProgress.Instance.pizzaPickedUp = false;

        GameProgress.Instance.secondDeliveryCompleted = true;  

        DeliveryManager.Instance.CompleteSecondDelivery();
    }
}