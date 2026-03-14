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

    private enum DeliveryType { None, OnlyPizza, PizzaAndCookie }
    private DeliveryType currentDeliveryType = DeliveryType.None;

    private Transform player;
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

        bool hasBurger = GameProgress.Instance != null && GameProgress.Instance.burgerPickedUp;
        bool hasPizza = GameProgress.Instance != null && GameProgress.Instance.pizzaPickedUp;
        bool hasCookie = GameProgress.Instance != null && GameProgress.Instance.cookiePickedUp;

        Debug.Log("Amy Interact | Burger: " + hasBurger + " | Pizza: " + hasPizza + " | Cookie: " + hasCookie);

        currentDeliveryType = DeliveryType.None;

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

        if (hasPizza && hasCookie)
        {
            Debug.Log("Amy dialogue branch: pizza + cookie");
            DialogueManager.Instance.StartDialogue(amy_pizza_plus_cookie);
            waitingForClickDelivery = true;
            currentDeliveryType = DeliveryType.PizzaAndCookie;
            return;
        }

        if (hasPizza)
        {
            Debug.Log("Amy dialogue branch: pizza only");
            DialogueManager.Instance.StartDialogue(amy_手上只有pizza);
            waitingForClickDelivery = true;
            currentDeliveryType = DeliveryType.OnlyPizza;
            return;
        }

        DialogueManager.Instance.StartDialogue(amy_手上无东西);
    }

    private void OnMouseDown()
    {
        if (!waitingForClickDelivery) return;
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive) return;

        Debug.Log("Amy clicked to complete delivery");

        CompleteDelivery();
        waitingForClickDelivery = false;
    }

    private void CompleteDelivery()
    {
        OrderManager.Instance.SubmitOrder();

        if (pizzaItem != null)
            pizzaItem.RemoveFromInventory();

        if (cookieItem != null && currentDeliveryType == DeliveryType.PizzaAndCookie)
            cookieItem.RemoveFromInventory();

        GameProgress.Instance.pizzaPickedUp = false;
        if (currentDeliveryType == DeliveryType.PizzaAndCookie)
            GameProgress.Instance.cookiePickedUp = false;

        GameProgress.Instance.secondDeliveryCompleted = true;

        int tip = 0;
        switch (currentDeliveryType)
        {
            case DeliveryType.OnlyPizza:
                tip = 0; 
                break;
            case DeliveryType.PizzaAndCookie:
                tip = 20; 
                break;
        }
        
        DeliveryManager.Instance.AddEarnings(tip);
        Debug.Log($"完成配送，小费：{tip}（配送类型：{currentDeliveryType}）");

        DeliveryManager.Instance.CompleteSecondDelivery();
        currentDeliveryType = DeliveryType.None;
    }
}