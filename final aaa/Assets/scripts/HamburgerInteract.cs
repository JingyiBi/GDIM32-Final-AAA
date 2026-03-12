using UnityEngine;

public class HamburgerInteract : InteractableBase
{
    public bool isPicked = false;
    public GameObject inventoryIcon;

    void OnMouseDown()
    {
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

            Debug.Log("Hamburger picked up!");
        }
    }
}