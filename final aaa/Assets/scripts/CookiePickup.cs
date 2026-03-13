using UnityEngine;

public class CookiePickup : MonoBehaviour
{
    public GameObject inventoryIcon;
    public float cookieInteractDistance = 4f;

    private Transform player;
    private bool hasPickedUp = false;

    public class TestClick : MonoBehaviour
    {
        void OnMouseDown()
        {
            Debug.Log("CLICK COOKIE");
        }
    }
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (inventoryIcon != null)
            inventoryIcon.SetActive(false);
    }

    void OnMouseDown()
    {
        Debug.Log("Cookie Clicked");
        if (hasPickedUp) return;
        if (!GameProgress.Instance.secondOrderAccepted) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > cookieInteractDistance) return;

        PickupCookie(); ;
    }

    void PickupCookie()
    {
        hasPickedUp = true;

        GameProgress.Instance.cookiePickedUp = true;

        if (inventoryIcon != null)
            inventoryIcon.SetActive(true);

        gameObject.SetActive(false);
    }

    public void RemoveFromInventory()
    {
        hasPickedUp = false;

        GameProgress.Instance.cookiePickedUp = false;

        if (inventoryIcon != null)
            inventoryIcon.SetActive(false);
    }

}