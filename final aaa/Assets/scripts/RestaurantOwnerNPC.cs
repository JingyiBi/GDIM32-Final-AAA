using UnityEngine;
public class RestaurantOwnerNPC : MonoBehaviour
{
    [Header("Interaction")]
    public float promptDisplayDistance = 7f; 
    public float interactTriggerDistance = 10f; 
    public GameObject interactionPrompt;
    public float sightAngleThreshold = 0.9f; 

    [Header("Dialogue Nodes")]
    public DialogueNode RO_BurgerOrder;          
    public DialogueNode RO_PizzaOrder_Line1;    
    public DialogueNode RO_PizzaOrder_Line2;    
    public DialogueNode RO_PizzaAndBurger;      
    public DialogueNode RO_Finish_PizzaOrder;   

    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInPromptRange; 

    private bool startedBurgerDialogue = false;
    private bool startedPizzaDialogue = false;
    private bool hasGivenBurgerReward = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
            DialogueManager.Instance.OnEnterDialogueNode += HandleEnterDialogueNode;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToNPC = (transform.position - player.position).normalized;
        float dotProduct = Vector3.Dot(player.forward, directionToNPC);
        bool isLookingAtNPC = dotProduct > sightAngleThreshold;
        
        Vector3 rayOrigin = player.position + Vector3.up;
        int ignoreLayers = LayerMask.GetMask("Player", "NPC", "Interactable");
        Ray ray = new Ray(rayOrigin, directionToNPC);
        bool hasObstacle = Physics.Raycast(ray, distance, ~ignoreLayers);
        
        isInPromptRange = distance <= promptDisplayDistance && isLookingAtNPC && !hasObstacle;
        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInPromptRange);

        bool canInteractByKey = distance <= interactTriggerDistance;
        if (!canInteractByKey) return;

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
        bool hasBurger = GameProgress.Instance.burgerPickedUp;
        bool hasPizza = GameProgress.Instance.pizzaPickedUp;

        // 第一个订单完成且空手的场景
        bool isFirstOrderDoneAndEmptyHanded = 
            GameProgress.Instance.firstDeliveryCompleted && 
            !hasBurger && !hasPizza && 
            OrderManager.Instance.currentOrder == null;
        
        if (isFirstOrderDoneAndEmptyHanded)
        {
            if (RO_PizzaAndBurger != null) DialogueManager.Instance.StartDialogue(RO_PizzaAndBurger);
            return;
        }

        if ((hasBurger && !GameProgress.Instance.firstDeliveryCompleted) || 
            (hasPizza && !GameProgress.Instance.secondDeliveryCompleted))
        {
            if (RO_PizzaAndBurger != null) DialogueManager.Instance.StartDialogue(RO_PizzaAndBurger);
            return;
        }

        bool isSecondOrderEmptyHanded = 
            state == GameState.SecondOrder && 
            !hasPizza && !hasBurger && 
            GameProgress.Instance.secondOrderAccepted;
        if (isSecondOrderEmptyHanded)
        {
            if (RO_PizzaAndBurger != null) DialogueManager.Instance.StartDialogue(RO_PizzaAndBurger);
            return;
        }

        bool isPizzaFinished = (state == GameState.SecondOrder && 
                                OrderManager.Instance.currentOrder != null && 
                                OrderManager.Instance.currentOrder.currentState == OrderState.Submitted);

        if (state == GameState.Finished || isPizzaFinished)
        {
            DeliveryManager.Instance.currentGameState = GameState.Finished;
            GameProgress.Instance.secondDeliveryCompleted = true;
            if (RO_Finish_PizzaOrder != null) DialogueManager.Instance.StartDialogue(RO_Finish_PizzaOrder);
        }
        else if (state == GameState.Transition)
        {
            startedPizzaDialogue = true;
            GameProgress.Instance.secondOrderAccepted = true;
            if (RO_PizzaOrder_Line1 != null) DialogueManager.Instance.StartDialogue(RO_PizzaOrder_Line1);
        }
        else if (state == GameState.FirstOrder)
        {
            if (GameProgress.Instance.firstOrderAccepted && !GameProgress.Instance.burgerPickedUp)
            {
                if (RO_PizzaAndBurger != null) DialogueManager.Instance.StartDialogue(RO_PizzaAndBurger);
                return;
            }
            
            if (OrderManager.Instance.currentOrder != null)
            {
                if (RO_PizzaAndBurger != null) DialogueManager.Instance.StartDialogue(RO_PizzaAndBurger);
            }
            else 
            {
                startedBurgerDialogue = true;
                GameProgress.Instance.hasTalkedToOwner = true;
                GameProgress.Instance.firstOrderAccepted = true;
                if (RO_BurgerOrder != null) DialogueManager.Instance.StartDialogue(RO_BurgerOrder);
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
            GameProgress.Instance.firstOrderAccepted = true;
        }
        else if (startedPizzaDialogue)
        {
            startedPizzaDialogue = false;
            DeliveryManager.Instance.StartPizzaPhase();
        }
    }

    private void HandleEnterDialogueNode(DialogueNode enteredNode)
    {
        if (enteredNode == RO_PizzaOrder_Line2 && !hasGivenBurgerReward && GameProgress.Instance.firstDeliveryCompleted)
        {
            DeliveryManager.Instance.totalEarnings += 50;
            GameProgress.Instance.firstOrderRewardClaimed = true;
            hasGivenBurgerReward = true;
            EarningsUI earningsUI = FindObjectOfType<EarningsUI>();
            if (earningsUI != null) earningsUI.RefreshDisplay();
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
            DialogueManager.Instance.OnEnterDialogueNode -= HandleEnterDialogueNode;
        }
    }
}