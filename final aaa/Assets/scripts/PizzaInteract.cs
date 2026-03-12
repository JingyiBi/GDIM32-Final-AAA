using UnityEngine;

public class PizzaInteract : InteractableBase
{
    public bool isPicked = false;
    public GameObject inventoryIcon;
    [Header("Interaction Settings")]
    public float pizzaInteractDistance = 6f; 
    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (isPicked || player == null || interactionPrompt == null || !GameProgress.Instance.secondOrderAccepted)
        {
            interactionPrompt?.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool isInRange = distance <= pizzaInteractDistance;
        interactionPrompt.SetActive(isInRange);
    }

    void OnMouseDown()
    {
        if (isPicked || player == null || !GameProgress.Instance.secondOrderAccepted) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        bool canInteract = distance <= pizzaInteractDistance;
        
        if (canInteract)
            Interact();
    }

    public override void Interact()
    {
        if (isPicked) return;

        if (OrderManager.Instance.currentOrder != null && 
            OrderManager.Instance.currentOrder.foodType == "Pizza")
        {
            isPicked = true;
            gameObject.SetActive(false);
            if (inventoryIcon != null) inventoryIcon.SetActive(true);
            OrderManager.Instance.PickUpOrder();
            GameProgress.Instance.pizzaPickedUp = true;
        }
    }

    public void RemoveFromInventory()
    {
        if (inventoryIcon != null)
        {
            inventoryIcon.SetActive(false);
            isPicked = false;
        }
        GameProgress.Instance.pizzaPickedUp = false;
    }
}