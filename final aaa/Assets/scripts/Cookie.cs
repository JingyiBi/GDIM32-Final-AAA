using UnityEngine;

public class PlayerFortuneCookie : MonoBehaviour
{
    public float pickUpDistance = 2f;
    private KeyCode pickUpKey = KeyCode.E;
    public Transform fortuneCookieTable;
    public CustomerAnton customerNPC;
    private bool hasCookie;

    private void Update()
    {
        if (Vector3.Distance(transform.position, fortuneCookieTable.position) <= pickUpDistance && Input.GetKeyDown(pickUpKey) && !hasCookie)
        {
            PickUpCookie();
        }
    }

    private void PickUpCookie()
    {
        hasCookie = true;

        DeliveryManager.Instance.currentOrder.isCookiePicked = true;

        Debug.Log("Fortune Cookie Picked Up! Extra Tip on Delivery!");
    }
}