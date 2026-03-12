using UnityEngine;

public class HamburgerInteract : InteractableBase
{
    public bool isPicked = false;
    public GameObject inventoryIcon;

    private Transform player;


    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void OnMouseDown()
    {
        if (isPicked) return;
        Debug.Log("Burger clicked");
        Interact();
    }

    public override void Interact()
    {
        if (isPicked) return;

        if (OrderManager.Instance.currentOrder != null &&
            OrderManager.Instance.currentOrder.foodType == "Burger")
        {
            isPicked = true;

            gameObject.SetActive(false);

            if (inventoryIcon != null)
                inventoryIcon.SetActive(true);

            OrderManager.Instance.PickUpOrder();
            GameProgress.Instance.burgerPickedUp = true;
            Debug.Log("Hamburger picked up!");
        }
    }
    public void RemoveFromInventory()
    {

        if (inventoryIcon != null)
        {
            inventoryIcon.SetActive(false);
            isPicked = false;
        }

        Debug.Log("Burger removed from inventory");
    }
}