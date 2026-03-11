using UnityEngine;

public class AmyInteract : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode noPizzaNode;
    public DialogueNode givePizzaNode;
    
    [Header("Dependencies")]
    public PizzaInteract pizzaItem;
    public CookiePickup cookieItem; 

    public override void Interact()
    {
        
        if (pizzaItem != null && pizzaItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(givePizzaNode);
            OrderManager.Instance.SubmitOrder();
            
            
            if (cookieItem != null && cookieItem.hasPickedUp)
            {
                DeliveryManager.Instance.AddEarnings(10);
            }
        }
        else
        {
            DialogueManager.Instance.StartDialogue(noPizzaNode);
        }
    }
}