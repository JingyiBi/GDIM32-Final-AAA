using UnityEngine;
using UnityEngine.UI;

public class CookieInteract : MonoBehaviour
{
    public Sprite cookieUISprite;
    public Transform bottomLeftUIContainer;
    public float interactionDistance = 3f;
    private Transform player;
    private bool isPicked = false;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (isPicked || player == null) return;

        if (DeliveryManager.Instance.currentGameState == GameState.SecondOrder)
        {
            if (Vector3.Distance(transform.position, player.position) <= interactionDistance)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                    {
                        PickUp();
                    }
                }
            }
        }
    }

    void PickUp()
    {
        isPicked = true;
        gameObject.SetActive(false);

        GameObject uiIcon = new GameObject("CookieIcon");
        uiIcon.transform.SetParent(bottomLeftUIContainer, false);
        Image img = uiIcon.AddComponent<Image>();
        img.sprite = cookieUISprite;
        img.preserveAspect = true;
        
        RectTransform rt = img.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 100);
    }
}