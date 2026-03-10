using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionDistance = 7f;
    public GameObject interactionPrompt;

    [Header("Dialogue Nodes")]
    public DialogueNode burgerStartNode;
    public DialogueNode pizzaStartNode;
    public DialogueNode finishPizzaNode;
    public DialogueNode wrongReturnNode;

    [Header("Food References")]
    public HamburgerInteract hamburgerInteract;
    public PizzaInteract pizzaInteract;

    private KeyCode interactKey = KeyCode.I;
    private Transform player;

    private bool isInRange;

    public bool isOrderAssigned;

    private bool startedBurgerDialogue;
    private bool startedPizzaDialogue;
    private bool startedFinishDialogue;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found! Make sure Player has tag 'Player'.");
            return;
        }

        player = playerObj.transform;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
    }

    private void Update()
    {
        if (player == null)
            return;

        CheckInteractionRange();

        if (!isInRange)
            return;

        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            StartOwnerDialogue();
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    private void StartOwnerDialogue()
    {
        if (DialogueManager.Instance == null || DeliveryManager.Instance == null)
            return;

        startedBurgerDialogue = false;
        startedPizzaDialogue = false;
        startedFinishDialogue = false;

        bool hasBurger = hamburgerInteract != null && hamburgerInteract.HasHamburger();
        bool hasPizza = pizzaInteract != null && pizzaInteract.HasPizza();

  
        if (hasBurger || hasPizza)
        {
            if (wrongReturnNode != null)
            {
                DialogueManager.Instance.StartDialogue(wrongReturnNode);
                return;
            }
        }


        if (DeliveryManager.Instance.secondDeliveryCompleted)
        {
            if (finishPizzaNode != null)
            {
                startedFinishDialogue = true;
                DialogueManager.Instance.StartDialogue(finishPizzaNode);
                return;
            }
        }


        if (DeliveryManager.Instance.firstDeliveryCompleted)
        {
            if (pizzaStartNode != null)
            {
                startedPizzaDialogue = true;
                DialogueManager.Instance.StartDialogue(pizzaStartNode);
                return;
            }
        }

  
        {
            startedBurgerDialogue = true;
            DialogueManager.Instance.StartDialogue(burgerStartNode);
        }
    }

    private void HandleDialogueEnd()
    {
        if (DeliveryManager.Instance == null)
            return;

        if (startedBurgerDialogue)
        {
            isOrderAssigned = true;
            startedBurgerDialogue = false;

            Debug.Log("Burger order assigned.");
        }

        if (startedPizzaDialogue)
        {
            isOrderAssigned = true;
            startedPizzaDialogue = false;

            Debug.Log("Pizza order assigned.");
        }

        if (startedFinishDialogue)
        {
            startedFinishDialogue = false;

            Debug.Log("Pizza storyline finished.");
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
    }
}