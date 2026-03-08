using UnityEngine;

public class RestaurantOwnerNPC : MonoBehaviour
{
    public float interactionDistance = 7f;
    public GameObject interactionPrompt;

    public DialogueNode burgerStartNode;
    public DialogueNode pizzaStartNode;

    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;

    public bool isOrderAssigned;

    private bool startedBurgerDialogue;
    private bool startedPizzaDialogue;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found! Make sure Player has the tag 'Player'.");
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
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance is null.");
            return;
        }

        if (DeliveryManager.Instance == null)
        {
            Debug.LogError("DeliveryManager.Instance is null.");
            return;
        }

        startedBurgerDialogue = false;
        startedPizzaDialogue = false;

        if (DeliveryManager.Instance.firstDeliveryCompleted)
        {
            if (pizzaStartNode == null)
            {
                Debug.LogWarning("Pizza Start Node is not assigned on RestaurantOwnerNPC.");
                return;
            }

            startedPizzaDialogue = true;
            DialogueManager.Instance.StartDialogue(pizzaStartNode);
        }
        else
        {
            if (burgerStartNode == null)
            {
                Debug.LogWarning("Burger Start Node is not assigned on RestaurantOwnerNPC.");
                return;
            }

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
            Debug.Log("Burger order assigned. Player can now pick up the burger.");
        }

        if (startedPizzaDialogue)
        {
            startedPizzaDialogue = false;
            Debug.Log("Pizza dialogue finished.");
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
    }
}