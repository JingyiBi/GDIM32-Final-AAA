using UnityEngine;

public class AmyInteract : InteractableBase
{
    [Header("Dialogue Resources")]
    public DialogueNode amy_手上无东西;     
    public DialogueNode amy_手上有汉堡时;       
    public DialogueNode amy_手上只有pizza;       
    public DialogueNode amy_pizza_plus_cookie;  

    [Header("Dependencies")]
    public PizzaInteract pizzaItem;
    public CookiePickup cookieItem;

    [Header("Interaction Settings")]
    public float sightAngleThreshold = 0.7f;
    public float promptDisplayDistance = 5f; 
    public float interactTriggerDistance = 8f; 
    private Transform player;
    private bool isInRange;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    private new void Update()
    {
        if (player == null || interactionPrompt == null) return;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAt = dotProduct > sightAngleThreshold;
        
        Vector3 rayOrigin = player.position + Vector3.up;
        int ignoreLayers = LayerMask.GetMask("Player", "NPC", "Interactable");
        Ray ray = new Ray(rayOrigin, directionToNPC);
        bool hasObstacle = Physics.Raycast(ray, distance, ~ignoreLayers);
        
        isInRange = distance <= promptDisplayDistance && isLookingAt && !hasObstacle;
        interactionPrompt.SetActive(isInRange);

        bool canInteractByKey = distance <= interactTriggerDistance && !DialogueManager.Instance.IsDialogueActive;
        if (canInteractByKey && Input.GetKeyDown(KeyCode.I))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        if (!GameProgress.Instance.hasTalkedToOwner)
        {
            DialogueManager.Instance.StartDialogue(amy_手上无东西);
            return;
        }

        if (GameProgress.Instance.burgerPickedUp)
        {
            DialogueManager.Instance.StartDialogue(amy_手上有汉堡时);
            return;
        }

        if (pizzaItem != null && pizzaItem.isPicked)
        {
            if (cookieItem != null && GameProgress.Instance.cookiePickedUp && !GameProgress.Instance.pizzaTipClaimed)
            {
                DialogueManager.Instance.StartDialogue(amy_pizza_plus_cookie);
                DeliveryManager.Instance.AddEarnings(10);
                GameProgress.Instance.pizzaTipClaimed = true;
            }
            else
            {
                DialogueManager.Instance.StartDialogue(amy_手上只有pizza);
            }
            OrderManager.Instance.SubmitOrder();
            GameProgress.Instance.pizzaPickedUp = false;
            GameProgress.Instance.secondDeliveryCompleted = true;
        }
        else
        {
            DialogueManager.Instance.StartDialogue(amy_手上无东西);
        }
    }
}