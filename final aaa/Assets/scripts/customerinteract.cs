using UnityEngine;

public class CustomerInteract : MonoBehaviour
{
    public HamburgerInteract hamburgerInteract;
    public RestaurantOwnerNPC restaurantOwner;
    public Transform inventoryUIContainer;
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;
    public EarningsUI earningsUI;
    private KeyCode interactKey = KeyCode.I;
    private Transform player;
    private bool isInRange;
    private bool isOrderDelivered = false;
    private bool isDialogueCompleted = false;
    private DialogueNode startNode;
    private bool isRewardGiven = false;
    private bool isDialogueStarted = false;

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

        if (isInRange && Input.GetKeyDown(interactKey))
        {
            if (hamburgerInteract != null && hamburgerInteract.HasHamburger())
            {
                if (!isDialogueStarted)
                {
                    DialogueManager.Instance.StartDialogue(startNode);
                    isDialogueStarted = true;
                }
            }
        }
    }

    private void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    private void OnDialogueCompletelyFinished()
    {
        // 核心逻辑：只有当你拿着汉堡且对话结束时触发
        if (hamburgerInteract != null && hamburgerInteract.HasHamburger() && !isRewardGiven)
        {
            // 1. 调用你原本的老板交单逻辑
            if (restaurantOwner != null)
            {
                restaurantOwner.SubmitOrder();
            }

            // 2. 移除汉堡 UI
            RemoveHamburgerUI();

            // 3. 【新增】仅通知 Manager 第一单完成了（为了开启 Pizza 流程）
            if (DeliveryManager.Instance != null)
            {
                DeliveryManager.Instance.CompleteFirstDelivery();
            }

            // 4. 更新奖励状态
            if (earningsUI != null) earningsUI.AddCurrentEarnings(50); 
            isRewardGiven = true;
            isOrderDelivered = true;
        }
    }

    private DialogueNode BuildDialogueTree()
    {
        // 严格还原你代码中的对话结构
        DialogueNode fourthNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "oh",
            endsDialogue = true,
            autoContinue = false
        };

        DialogueNode firstNode = new DialogueNode
        {
            speakerName = "Anton",
            dialogueText = "Thank you for the delivery. Click on me and your meal will be delivered. I will give you 50 dollars.",
            endsDialogue = false,
            autoContinue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "OK", // 你的 Continue 按钮
                    nextNode = fourthNode
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

    private System.Collections.IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueManager.Instance.EndDialogue();
    }
}