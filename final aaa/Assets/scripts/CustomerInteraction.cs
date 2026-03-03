using UnityEngine;
using UnityEngine.UI; // 若用Text/Image需加

public class CustomerInteract : MonoBehaviour
{
    // 原有字段保持不变
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public float interactionDistance = 5f;
    public GameObject interactionPrompt; // 浮窗UI
    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool isOrderDelivered = false;
    private bool isDialogueCompleted = false;
    private DialogueNode startNode;

    // 新增：若浮窗是世界空间UI，可绑定到Anton的头顶
    public Vector3 promptOffset = new Vector3(0, 2f, 0); // 浮窗偏移（头顶2米）

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        player = playerObj.transform;

        startNode = BuildDialogueTree();

        // 初始隐藏浮窗
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        DialogueManager.Instance.OnDialogueEnd += OnDialogueCompletelyFinished;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnd -= OnDialogueCompletelyFinished;
        }
    }

    private void Update()
    {
        if (isOrderDelivered) return;

        CheckInteractionRange();

        // 按I键触发对话
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            StartCustomerDialogue();
            // 触发对话后隐藏浮窗
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }

        // 原有点击交付订单逻辑保持不变
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject 
                    && hamburgerInteract != null 
                    && restaurantOwner != null 
                    && restaurantOwner.isOrderAssigned 
                    && isDialogueCompleted)
                {
                    RemoveHamburgerUI();
                    restaurantOwner.SubmitOrder();
                    isOrderDelivered = true;
                    if (interactionPrompt != null)
                        interactionPrompt.SetActive(false);
                }
            }
        }

        // 新增：如果是世界空间UI，让浮窗跟随Anton
        if (interactionPrompt != null && interactionPrompt.activeSelf)
        {
            interactionPrompt.transform.position = transform.position + promptOffset;
            // 可选：让浮窗始终朝向玩家/相机
            interactionPrompt.transform.LookAt(player);
            interactionPrompt.transform.rotation = Quaternion.LookRotation(player.forward);
        }
    }

    // 完善范围检测和浮窗显示逻辑
    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        // 仅在“范围内+未完成订单+未触发对话”时显示浮窗
        bool shouldShowPrompt = isInRange && !isOrderDelivered && !DialogueManager.Instance.IsDialogueActive();
        if (interactionPrompt != null)
            interactionPrompt.SetActive(shouldShowPrompt);
    }

    // 原有方法保持不变
    private void StartCustomerDialogue()
    {
        DialogueManager.Instance.StartDialogue(startNode);
        hamburgerInteract.hasTalkedToCustomer = true;
    }

    private void OnDialogueCompletelyFinished()
    {
        isDialogueCompleted = true;
    }

    private DialogueNode BuildDialogueTree()
    {
        // 原有对话树逻辑不变
        DialogueNode thirdNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Go to the boss to claim your payment.",
            endsDialogue = true,
            autoContinue = false
        };

        DialogueNode secondNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Click on me and your meal will be delivered.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = thirdNode
                }
            }
        };

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you for the delivery.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "Continue",
                    nextNode = secondNode
                }
            }
        };

        return firstNode;
    }

    private void RemoveHamburgerUI()
    {
        if (inventoryUIContainer == null) return;
        
        Transform hamburgerIcon = inventoryUIContainer.Find("HamburgerIcon");
        if (hamburgerIcon != null)
        {
            Destroy(hamburgerIcon.gameObject);
        }
        if (hamburgerInteract != null)
        {
            hamburgerInteract.RemoveHamburgerIcon();
        }
    }