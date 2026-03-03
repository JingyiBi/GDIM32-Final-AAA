using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    public float interactionDistance = 5f;
    public int tipAmount = 20;
    public int fortuneCookieExtraTip = 10;

    private KeyCode interactKey = KeyCode.I;
    private KeyCode deliverKey = KeyCode.E;

    private Transform player;
    private bool isInRange;
    private bool hasDelivered;
    private bool isDialogueOpen;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hasDelivered = false;
        isDialogueOpen = false;
    }

    private void Update()
    {
        CheckInteractionRange();

        if (!isInRange) return;

        if (Input.GetKeyDown(interactKey) &&
            !hasDelivered &&
            DeliveryManager.Instance.currentOrder != null)
        {
            isDialogueOpen = true;
            InitiateDialogue();
        }

        if (isDialogueOpen &&
            Input.GetKeyDown(deliverKey) &&
            !hasDelivered &&
            DeliveryManager.Instance.currentOrder != null &&
            DeliveryManager.Instance.currentOrder.currentState == OrderState.PickedUp)
        {
            DeliverFood();
            isDialogueOpen = false;
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;
    }

    private void InitiateDialogue()
    {
        Debug.Log("Customer: Where's my food? Press E to deliver!");
    }

    private void DeliverFood()
    {
        if (DeliveryManager.Instance.currentOrder.currentState != OrderState.PickedUp)
            return;

        bool hasCookie = DeliveryManager.Instance.currentOrder.isCookiePicked;

        int totalTip = hasCookie ? tipAmount + fortuneCookieExtraTip : tipAmount;

        DeliveryManager.Instance.AddEarnings(totalTip);
        FindObjectOfType<EarningsUI>().AddCurrentEarnings(totalTip);

        DeliveryManager.Instance.currentOrder.currentState = OrderState.Delivered;

        hasDelivered = true;

        Debug.Log("Food Delivered! Tip: " + totalTip);
        Debug.Log("Return to Owner to submit order!");
    }
}