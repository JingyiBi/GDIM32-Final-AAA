using UnityEngine;

public class CustomerAnton : InteractableBase
{
    public DialogueNode welcomeNode;
    public DialogueNode thankYouNode;
    public HamburgerInteract hamburgerItem;

    public override void Interact()
    {
        if (hamburgerItem != null && hamburgerItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(thankYouNode);
            OrderManager.Instance.SubmitOrder();
            DeliveryManager.Instance.CompleteFirstDelivery();
        }
        else
        {
            DialogueManager.Instance.StartDialogue(welcomeNode);
        }
    }
}