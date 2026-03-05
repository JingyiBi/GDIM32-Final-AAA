using UnityEngine;

public class AmyInteract : MonoBehaviour
{
    public float interactDistance = 10f;
    private Transform player;
    private bool isFinished = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (isFinished) return;

        
        if (DeliveryManager.Instance.currentGameState == GameState.SecondOrder)
        {
            if (Vector3.Distance(transform.position, player.position) <= interactDistance)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    DeliverToAmy();
                }
            }
        }
    }

    void DeliverToAmy()
    {
        isFinished = true;

        
        RestaurantOwnerNPC owner = FindObjectOfType<RestaurantOwnerNPC>();
        
        GameObject pizza = GameObject.Find("whole pepperoni"); 
        if (pizza != null) pizza.SetActive(false);

        
    }
}
