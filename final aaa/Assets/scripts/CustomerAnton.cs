using UnityEngine;

public class CustomerAnton : InteractableBase
{
    public DialogueNode nothingNode;
    public DialogueNode burgerNode;
    public DialogueNode pizzaNode;

    public HamburgerInteract hamburgerItem;
    public PizzaInteract pizzaItem;

    private bool deliveryDialoguePlayed = false;
    private bool orderDelivered = false;
    private bool waitingForClickDelivery = false;

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