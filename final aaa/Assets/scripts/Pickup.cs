using UnityEngine;

public class PickupZone : MonoBehaviour
{
    public float pickupDistance = 3f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (DeliveryManager.Instance.currentOrder == null) return;

        if (DeliveryManager.Instance.currentOrder.currentState != OrderState.Accepted) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupDistance && Input.GetKeyDown(KeyCode.E))
        {
            DeliveryManager.Instance.currentOrder.currentState = OrderState.PickedUp;
            Debug.Log("Order Picked Up!");
            Debug.Log("Mouse click detected");
        }
    }
}