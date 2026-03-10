using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    [Header("References")]
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public EarningsUI earningsUI;
    public GameObject returnToOwnerPromptPanel;

    [Header("Dialogue Nodes")]
    public DialogueNode burgerDialogue;
    public DialogueNode pizzaDialogue;
    public DialogueNode nothingDialogue;

    [Header("Interaction")]
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;
    private KeyCode interactKey = KeyCode.I;

    private Transform player;

    private bool isInRange;
    private bool isDialogueStarted;
    private bool isDialogueCompleted;
    private bool isOrderDelivered;
    private bool isRewardGiven;

    public float promptDisplayDuration = 3f;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player = playerObj.transform;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (returnToOwnerPromptPanel != null)
            returnToOwnerPromptPanel.SetActive(false);

        DialogueManager.Instance.OnDialogueEnd += OnDialogueCompletelyFinished;
    }

    void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueCompletelyFinished;
    }

    void Update()
    {
        if (isOrderDelivered) return;

        CheckInteractionRange();

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            StartAntonDialogue();
        }
    }

    void StartAntonDialogue()
    {
        if (hamburgerInteract == null) return;

        if (!restaurantOwner.isOrderAssigned)
        {
            Debug.Log("You don't have an order yet.");
            return;
        }

        if (hamburgerInteract.HasHamburger())
        {
            DialogueManager.Instance.StartDialogue(burgerDialogue);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(nothingDialogue);
        }

        isDialogueStarted = true;
    }

    void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    void OnDialogueCompletelyFinished()
    {
        if (!isDialogueStarted) return;

        if (hamburgerInteract != null && hamburgerInteract.HasHamburger() && !isRewardGiven)
        {
            DeliverOrder();
        }

        isDialogueCompleted = true;
    }

    void DeliverOrder()
    {
        RemoveHamburgerUI();

        if (OrderManager.Instance != null)
            OrderManager.Instance.SubmitOrder();

        if (DeliveryManager.Instance != null)
            DeliveryManager.Instance.CompleteFirstDelivery();

        if (earningsUI != null)
            earningsUI.AddCurrentEarnings(50);

        isRewardGiven = true;
        isOrderDelivered = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (returnToOwnerPromptPanel != null)
        {
            returnToOwnerPromptPanel.SetActive(true);
            StartCoroutine(HidePromptAfterDelay(promptDisplayDuration));
        }

        Debug.Log("Burger delivered successfully!");
    }

    void RemoveHamburgerUI()
    {
        if (inventoryUIContainer == null) return;

        Transform hamburgerIcon = inventoryUIContainer.Find("HamburgerIcon");

        if (hamburgerIcon != null)
        {
            Destroy(hamburgerIcon.gameObject);
        }

        if (hamburgerInteract != null)
        {
            hamburgerInteract.RemoveHamburgerIcon();
        }
    }

    System.Collections.IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (returnToOwnerPromptPanel != null)
            returnToOwnerPromptPanel.SetActive(false);
    }
}