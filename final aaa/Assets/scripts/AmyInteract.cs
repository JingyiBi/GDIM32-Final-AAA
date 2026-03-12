using UnityEngine;

public class AmyInteract : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode noPizzaNode;
    public DialogueNode givePizzaNode;
    
    [Header("Dependencies")]
    public PizzaInteract pizzaItem;
    public CookiePickup cookieItem;

    [Header("Interaction Settings")]
    public float sightAngleThreshold = 0.9f;
    private Transform player;
    private bool isInRange;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (player == null || interactionPrompt == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAt = dotProduct > sightAngleThreshold;
        
        Ray ray = new Ray(player.position, directionToNPC);
        bool hasObstacle = Physics.Raycast(ray, distance, ~LayerMask.GetMask("Player"));
        
        isInRange = distance <= interactionDistance && isLookingAt && !hasObstacle;

        interactionPrompt.SetActive(isInRange);

        if (isInRange && Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.IsDialogueActive)
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (pizzaItem != null && pizzaItem.isPicked)
        {
            DialogueManager.Instance.StartDialogue(givePizzaNode);
            OrderManager.Instance.SubmitOrder();
            
            if (cookieItem != null && cookieItem.hasPickedUp)
            {
                DeliveryManager.Instance.AddEarnings(10);
            }
        }
        else
        {
            DialogueManager.Instance.StartDialogue(noPizzaNode);
        }
    }
}