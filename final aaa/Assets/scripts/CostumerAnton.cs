using UnityEngine;

public class CustomerAnton : MonoBehaviour
{
    public float interactionDistance = 8f;
    private KeyCode interactKey = KeyCode.I;

    private Transform player;
    private bool isInRange;

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

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            TrySubmitOrder();
        }
    }

    void TrySubmitOrder()
    {
        if (DeliveryManager.Instance == null) return;

        if (DeliveryManager.Instance.currentOrder == null)
            return;

        if (DeliveryManager.Instance.currentOrder.foodType != "Burger")
            return;

        RestaurantOwnerNPC owner = FindObjectOfType<RestaurantOwnerNPC>();
        if (owner != null)
        {
            owner.SubmitOrder();
        }

        OrderUI inventory = FindObjectOfType<OrderUI>();
        if (inventory != null)
        {
            inventory.HideOrderUI();

            DialogueNode thankNode = new DialogueNode
            {
                speakerName = "Anton",
                dialogueText = "Thank you!",
                endsDialogue = true
            };

            DialogueManager.Instance.StartDialogue(thankNode);
        }
    }
}