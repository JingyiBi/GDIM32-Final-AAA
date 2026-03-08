using UnityEngine;
using UnityEngine.UI;

public class CookiePickup : MonoBehaviour
{
    public Image cookieUIImage;
    public Transform inventoryUIContainer;
    public float interactionDistance = 3f;
    public GameObject promptText;

    private Transform player;

    public bool hasPickedUp = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        
        if (cookieUIImage != null)
            cookieUIImage.gameObject.SetActive(false);
        
        if (promptText != null)
            promptText.SetActive(false);
    }

    private void Update()
    {
        if (player == null || hasPickedUp) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isInRange = distance <= interactionDistance;

        if (promptText != null)
            promptText.SetActive(isInRange);

        if (Input.GetMouseButtonDown(0))
        {
            CheckRaycastForCookie();
        }
    }
    public bool HasCookie()
    {
        return hasPickedUp;
    }
    private void CheckRaycastForCookie()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PickUpCookie();
            }
        }
    }

    private void PickUpCookie()
    {
        hasPickedUp = true;

        if (promptText != null)
            promptText.SetActive(false);
        
        if (cookieUIImage != null)
            cookieUIImage.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}