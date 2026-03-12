using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionDistance = 7f;
    public GameObject interactionPrompt;
    [Tooltip("range")]
    public float sightAngleThreshold = 0.9f; 

    [Header("Dialogue Nodes")]
    public DialogueNode burgerStartNode;
    public DialogueNode wrongReturnNode; 
    public DialogueNode pizzaStartNode;
    public DialogueNode finishPizzaNode;

    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;

    private bool startedBurgerDialogue = false;
    private bool startedPizzaDialogue = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAtNPC = dotProduct > sightAngleThreshold;
        
        Ray ray = new Ray(player.position, directionToNPC);
        bool hasObstacle = Physics.Raycast(ray, distance, ~LayerMask.GetMask("Player"));
        
        isInRange = distance <= interactionDistance && isLookingAtNPC && !hasObstacle;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);

        if (!isInRange) return;

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            InteractWithOwner();
        }
    }

    private void InteractWithOwner()
    {
        
        GameState state = DeliveryManager.Instance.currentGameState;

        
        bool isPizzaFinished = (state == GameState.SecondOrder && 
                                OrderManager.Instance.currentOrder != null && 
                                OrderManager.Instance.currentOrder.currentState == OrderState.Submitted);

        
        if (state == GameState.Finished || isPizzaFinished)
        {
            DeliveryManager.Instance.currentGameState = GameState.Finished;
            if (finishPizzaNode != null) DialogueManager.Instance.StartDialogue(finishPizzaNode);
        }
        
        else if (state == GameState.SecondOrder)
        {
            if (wrongReturnNode != null) DialogueManager.Instance.StartDialogue(wrongReturnNode);
        }
        
        else if (state == GameState.Transition)
        {
            startedPizzaDialogue = true;
            if (pizzaStartNode != null) DialogueManager.Instance.StartDialogue(pizzaStartNode);
        }
        
        else if (state == GameState.FirstOrder)
        {
            
            if (OrderManager.Instance.currentOrder != null)
            {
                if (wrongReturnNode != null) DialogueManager.Instance.StartDialogue(wrongReturnNode);
            }
            else 
            {
                startedBurgerDialogue = true;
                if (burgerStartNode != null) DialogueManager.Instance.StartDialogue(burgerStartNode);
            }
        }
    }

    private void HandleDialogueEnd()
    {
        
        if (startedBurgerDialogue)
        {
            startedBurgerDialogue = false;
            if (DeliveryManager.Instance.burgerOrder != null)
            {
                OrderManager.Instance.AcceptOrder(DeliveryManager.Instance.burgerOrder);
            }
        }
        else if (startedPizzaDialogue)
        {
            startedPizzaDialogue = false;
            DeliveryManager.Instance.StartPizzaPhase();
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
    }
}