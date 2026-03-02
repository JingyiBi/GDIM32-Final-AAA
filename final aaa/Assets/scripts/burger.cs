using UnityEngine;
using UnityEngine.UI;

public class HamburgerInteract : MonoBehaviour
{
    public Sprite hamburgerUISprite;
    public Transform inventoryUIContainer;
    private bool isPicked = false;
    private RestaurantOwnerNPC restaurantOwner;
    public bool hasTalkedToCustomer = false;

    void Start()
    {
        restaurantOwner = FindObjectOfType<RestaurantOwnerNPC>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPicked && restaurantOwner != null && restaurantOwner.isOrderAssigned)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    InteractWithHamburger();
                }
            }
        }
    }

    private void InteractWithHamburger()
    {
        gameObject.SetActive(false); 

        CreateHamburgerUI();

        isPicked = true;

        Debug.Log("汉堡已拾取！");
    }

    private void CreateHamburgerUI()
    {
        if (inventoryUIContainer == null || hamburgerUISprite == null)
        {
            Debug.LogError("请在Inspector面板中赋值汉堡UI贴图和InventoryUI容器！");
            return;
        }

        GameObject uiIcon = new GameObject("HamburgerIcon");
        uiIcon.transform.SetParent(inventoryUIContainer, false);

        Image image = uiIcon.AddComponent<Image>();
        image.sprite = hamburgerUISprite;
        image.preserveAspect = true; 

        RectTransform rect = uiIcon.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public void RemoveHamburgerIcon()
    {
        Transform icon = inventoryUIContainer.Find("HamburgerIcon");
        if (icon != null) Destroy(icon.gameObject);
    }
}