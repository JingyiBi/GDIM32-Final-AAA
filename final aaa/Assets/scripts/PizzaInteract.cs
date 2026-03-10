using UnityEngine;
using UnityEngine.UI;

public class PizzaInteract : MonoBehaviour
{
    public Sprite pizzaUISprite;
    public Transform inventoryUIContainer;
    public bool isPicked = false;
    public float interactionDistance = 4f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (isPicked) return;

        if (DeliveryManager.Instance.currentGameState == GameState.SecondOrder)
        {
            if (Vector3.Distance(transform.position, player.position) <= interactionDistance)
            {
                if (Input.GetMouseButtonDown(0)) 
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            PickUp();
                        }
                    }
                }
            }
        }
    }

    void PickUp()
    {
        isPicked = true;
        gameObject.SetActive(false);

        if (DeliveryManager.Instance.currentOrder != null)
        {
            DeliveryManager.Instance.currentOrder.currentState = OrderState.PickedUp;
            if (OrderUI.Instance != null) OrderUI.Instance.UpdateOrderUI(DeliveryManager.Instance.currentOrder);
        }

        GameObject uiIcon = new GameObject("PizzaIcon");
        uiIcon.transform.SetParent(inventoryUIContainer, false);
        Image img = uiIcon.AddComponent<Image>();
        img.sprite = pizzaUISprite;
        img.preserveAspect = true;
    }
    
    public bool HasPizza()
    {
        return isPicked;
    }
}