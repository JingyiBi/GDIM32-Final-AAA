using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject && hamburgerInteract != null && restaurantOwner != null && restaurantOwner.isOrderAssigned)
                {
                    RemoveHamburgerUI();
                    restaurantOwner.SubmitOrder();
                }
            }
        }
    }

    private void RemoveHamburgerUI()
    {
        if (inventoryUIContainer == null) return;
        
        Transform hamburgerIcon = inventoryUIContainer.Find("HamburgerIcon");
        if (hamburgerIcon != null)
        {
            Destroy(hamburgerIcon.gameObject);
        }
    }
}