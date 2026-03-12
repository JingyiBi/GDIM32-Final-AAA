using UnityEngine;

public class CustomerAnton : InteractableBase
{
    public DialogueNode nothingNode;
    public DialogueNode burgerNode;
    public DialogueNode pizzaNode;
    public HamburgerInteract hamburgerItem;
    public PizzaInteract pizzaItem;

    public override void Interact()
    {
        if (hamburgerItem != null && hamburgerItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(burgerNode);
            OrderManager.Instance.SubmitOrder();
            DeliveryManager.Instance.CompleteFirstDelivery();
        }

        else if (pizzaItem != null && pizzaItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(pizzaNode);
        }

        else
        {
            DialogueManager.Instance.StartDialogue(nothingNode);
        }
    }
}