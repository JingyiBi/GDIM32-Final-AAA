using UnityEngine;

public class CookieProximityTip : MonoBehaviour
{
    public GameObject tipUI; 
    public float showDistance = 4f;
    private Transform player;

    void Start() 
    {
        player = GameObject.FindWithTag("Player").transform;
        if(tipUI) tipUI.SetActive(false);
    }

    void Update()
    {
        
        if (GameProgress.Instance != null && GameProgress.Instance.secondOrderAccepted && !GameProgress.Instance.cookiePickedUp)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            tipUI.SetActive(dist <= showDistance);
        }
        else
        {
            if(tipUI) tipUI.SetActive(false);
        }
    }
}