using UnityEngine;
using UnityEngine.UI;

public class CustomerInteract : MonoBehaviour
{
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;
    public EarningsUI earningsUI;
    public GameObject returnToOwnerPromptPanel;
    public float promptDisplayDuration = 3f;
    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool isOrderDelivered = false;
    private bool isDialogueCompleted = false;
    private DialogueNode startNode;
    private bool isRewardGiven = false;
    private bool isDialogueStarted = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        player = playerObj.transform;

        startNode = BuildDialogueTree();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        if (returnToOwnerPromptPanel != null)
            returnToOwnerPromptPanel.SetActive(false);
        
        DialogueManager.Instance.OnDialogueEnd += OnDialogueCompletelyFinished;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueCompletelyFinished;
        }
    }

    private void Update()
    {
        if (isOrderDelivered) return;

        CheckInteractionRange();

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            if (hamburgerInteract != null && hamburgerInteract.HasHamburger())
            {
                if (!isDialogueStarted)
                {
                    DialogueManager.Instance.StartDialogue(startNode);
                    isDialogueStarted = true;
                }
            }
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    private void OnDialogueCompletelyFinished()
    {
        if (hamburgerInteract != null && hamburgerInteract.HasHamburger() && !isRewardGiven)
        {
            if (restaurantOwner != null)
            {
                restaurantOwner.SubmitOrder();
            }

            RemoveHamburgerUI();

            if (DeliveryManager.Instance != null)
            {
                DeliveryManager.Instance.CompleteFirstDelivery();
            }

            if (earningsUI != null) earningsUI.AddCurrentEarnings(50); 
            isRewardGiven = true;
            isOrderDelivered = true;
            
            if (returnToOwnerPromptPanel != null)
            {
                returnToOwnerPromptPanel.SetActive(true);
                StartCoroutine(HidePromptAfterDelay(promptDisplayDuration));
            }
        }
    }

    private DialogueNode BuildDialogueTree()
    {
        DialogueNode fourthNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "oh",
            endsDialogue = true,
            autoContinue = false
        };

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you for the delivery. Click on me and your meal will be delivered. I will give you 50 dollars.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "OK", 
                    nextNode = fourthNode
                }
            }
        };

        return firstNode;
    }

    private void RemoveHamburgerUI()
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

    private System.Collections.IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueManager.Instance.EndDialogue();
    }

    private System.Collections.IEnumerator HidePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (returnToOwnerPromptPanel != null)
            returnToOwnerPromptPanel.SetActive(false);
    }
}