using UnityEngine;

public class CookiePickup : InteractableBase
{
    public bool hasPickedUp = false;
    public GameObject cookieInventoryIcon; 

    public override void Interact()
    {
        if (hasPickedUp) return;

        hasPickedUp = true;
        gameObject.SetActive(false); 
        
        if (cookieInventoryIcon != null)
            cookieInventoryIcon.SetActive(true);

        Debug.Log("Picked up the cookie!");
    }
}