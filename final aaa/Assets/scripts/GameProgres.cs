using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [Header("Quest States")]
    // restaurant owner
    public bool hasTalkedToOwner = false;         
    public bool firstOrderAccepted = false;        
    public bool secondOrderAccepted = false;       

    // burger delivery
    public bool burgerPickedUp = false;            
    public bool firstDeliveryCompleted = false;    
    public bool firstOrderRewardClaimed = false;   

    // pizza delivery
    public bool pizzaPickedUp = false;            
    public bool secondDeliveryCompleted = false;   
    public bool secondOrderRewardClaimed = false;  

    // tips fortune cookie
    public bool cookiePickedUp = false;           
    public bool pizzaTipClaimed = false;         

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}