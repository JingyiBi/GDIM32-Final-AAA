using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    public float interactionDistance = 3f;
    public int tipAmount = 20;
    public int fortuneCookieExtraTip = 10;
    private KeyCode interactKey = KeyCode.I;
    private KeyCode deliverKey = KeyCode.E;
    private Transform player;
    private bool isInRange;
    private bool hasDelivered;
    public bool hasFortuneCookie;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hasDelivered = false;
        hasFortuneCookie = false;
    }

    private void Update()
    {
        CheckInteractionRange();
        if (isInRange && Input.GetKeyDown(interactKey) && !hasDelivered && DeliveryManager.Instance.currentOrder != null)
        {
            InitiateDialogue();
        }
        if (isInRange && Input.GetKeyDown(deliverKey) && !hasDelivered && DeliveryManager.Instance.currentOrder != null)
        {
            DeliverFood();
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
        int totalTip = hasFortuneCookie ? tipAmount + fortuneCookieExtraTip : tipAmount;
        DeliveryManager.Instance.AddEarnings(totalTip);
        FindObjectOfType<EarningsUI>().AddCurrentEarnings(totalTip);
        hasDelivered = true;
        Debug.Log("Food Delivered! Tip: " + totalTip);
        Debug.Log("Return to Owner to submit order!");
    }
}