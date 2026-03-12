using UnityEngine;

public class CookiePickup : MonoBehaviour
{
    public GameObject inventoryIcon;
    public bool hasPickedUp = false;
    public float cookieInteractDistance = 4f; 
    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (inventoryIcon != null)
            inventoryIcon.SetActive(false);
    }

    private void Update()
    {
        if (hasPickedUp || !GameProgress.Instance.secondOrderAccepted || player == null)
        {
            inventoryIcon?.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        inventoryIcon.SetActive(distance <= cookieInteractDistance);
    }

    private void OnMouseDown()
    {
        if (hasPickedUp || !GameProgress.Instance.secondOrderAccepted || player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > cookieInteractDistance) return;
        
        hasPickedUp = true;
        GameProgress.Instance.cookiePickedUp = true;
        gameObject.SetActive(false);
        
        if (inventoryIcon != null)
            inventoryIcon.SetActive(true);
    }

    public void RemoveFromInventory()
    {
        if (inventoryIcon != null)
        {
            inventoryIcon.SetActive(false);
            hasPickedUp = false;
        }
        GameProgress.Instance.cookiePickedUp = false;
        GameProgress.Instance.pizzaTipClaimed = false;
    }
}