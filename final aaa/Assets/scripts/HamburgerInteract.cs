using UnityEngine;

public class HamburgerInteract : InteractableBase
{
    public bool isPicked = false;
    public GameObject inventoryIcon;

    [Header("Sound")]
    public AudioClip pickupSFX;
    private AudioSource audioSource;

    [Header("Interaction Settings")]
    public float burgerInteractDistance = 6f; 
    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        { if (playerObj != null) player = playerObj.transform;

        audioSource = playerObj.GetComponent<AudioSource>();
        }
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (isPicked || player == null || interactionPrompt == null || !GameProgress.Instance.firstOrderAccepted)
        {
            interactionPrompt?.SetActive(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool isInRange = distance <= burgerInteractDistance;
        interactionPrompt.SetActive(isInRange);
    }

    void OnMouseDown()
    {
        if (isPicked || player == null || !GameProgress.Instance.firstOrderAccepted) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        bool canInteract = distance <= burgerInteractDistance;
        
        if (canInteract)
            Interact();
    }

    public override void Interact()
    {
        if (isPicked) return;

        if (OrderManager.Instance.currentOrder != null &&
            OrderManager.Instance.currentOrder.foodType == "Burger")
        {
            isPicked = true;
            if (audioSource != null && pickupSFX != null)
            {
                audioSource.PlayOneShot(pickupSFX);
            }

            gameObject.SetActive(false);
            if (inventoryIcon != null)
                inventoryIcon.SetActive(true);

            OrderManager.Instance.PickUpOrder();
            GameProgress.Instance.burgerPickedUp = true;
            Debug.Log("[汉堡拾取] burgerPickedUp已设为true");
    }
    }

    public void RemoveFromInventory()
    {
        if (inventoryIcon != null)
        {
            inventoryIcon.SetActive(false);
            isPicked = false;
        }
        GameProgress.Instance.burgerPickedUp = false;
    }
}