using UnityEngine;

public class PizzaInteract : InteractableBase
{
    public bool isPicked = false;
    public GameObject inventoryIcon;
    [Header("Interaction Settings")]
    public float pizzaInteractDistance = 4f;
    public float sightAngleThreshold = 0.9f;
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
        if (isPicked || player == null || interactionPrompt == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToItem = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToItem);
        bool isLookingAt = dotProduct > sightAngleThreshold;
        
        Ray ray = new Ray(player.position, directionToItem);
        bool hasObstacle = Physics.Raycast(ray, distance, ~LayerMask.GetMask("Player"));
        
        bool isInRange = distance <= pizzaInteractDistance && isLookingAt && !hasObstacle;

        interactionPrompt.SetActive(isInRange);
    }

    void OnMouseDown()
    {
        if (player == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToItem = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToItem);
        
        Ray ray = new Ray(player.position, directionToItem);
        bool hasObstacle = Physics.Raycast(ray, distance, ~LayerMask.GetMask("Player"));
        
        bool canInteract = distance <= pizzaInteractDistance && dotProduct > sightAngleThreshold && !hasObstacle;
        
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
        }
    }
}