using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [Header("Quest States")]
    public bool hasTalkedToOwner = false;
    public bool firstOrderAccepted = false;

    public bool burgerPickedUp = false;
    public bool firstDeliveryCompleted = false;
    public bool firstOrderRewardClaimed = false;

    public bool secondOrderAccepted = false;
    public bool pizzaPickedUp = false;
    public bool secondDeliveryCompleted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}