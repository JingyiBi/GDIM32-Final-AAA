using UnityEngine;

public class CustomerAnton : MonoBehaviour
{
    public float interactionDistance = 8f;
    private KeyCode interactKey = KeyCode.I;

    private Transform player;
    private bool isInRange;

    private bool dialogueFinished = false;

    public GameObject interactionHint;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionHint != null)
            interactionHint.SetActive(isInRange);

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            if (!dialogueFinished)
                PlayDialogue();
            else
                SubmitOrder();
        }
    }
    void PlayDialogue()
    {
        DialogueNode thankNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you.",
            endsDialogue = true,
            autoContinue = true
        };

        DialogueNode playerNode = new DialogueNode
        {
            speakerName = "You",
            dialogueText = "This is your Burger.",
            endsDialogue = false,
            autoContinue = true,
            choices = new DialogueChoice[]
            {
                new DialogueChoice { nextNode = thankNode }
            }
        };

        DialogueNode hiNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Hi!",
            endsDialogue = false,
            autoContinue = true,
            choices = new DialogueChoice[]
            {
                new DialogueChoice { nextNode = playerNode }
            }
        };

        DialogueManager.Instance.StartDialogue(hiNode);

        dialogueFinished = true;

        Debug.Log("Click Anton again to give him the burger.");
    }

    void SubmitOrder()
    {
        if (DeliveryManager.Instance == null) return;
        if (DeliveryManager.Instance.currentOrder == null) return;

        if (DeliveryManager.Instance.currentOrder.foodType != "Burger")
            return;

        RestaurantOwnerNPC owner = FindObjectOfType<RestaurantOwnerNPC>();
        if (owner != null)
            owner.SubmitOrder();

        OrderUI orderUI = FindObjectOfType<OrderUI>();
        if (orderUI != null)
            orderUI.HideOrderUI();

        dialogueFinished = false;
    }
}