using UnityEngine;

public class AmyInteract : MonoBehaviour
{
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;

    public PizzaInteract pizzaInteract;
    public CookiePickup cookiePickup;
    public RestaurantOwnerNPC ownerNPC;

    private Transform player;
    private bool isInRange;

    private KeyCode interactKey = KeyCode.I;

    private bool bonusGiven = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        player = playerObj.transform;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        CheckInteractionRange();

        if (!isInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartAmyDialogue();
        }
    }

    void CheckInteractionRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= interactionDistance;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(isInRange);
    }

    void StartAmyDialogue()
    {
        if (!pizzaInteract.isPicked)
        {
            DialogueManager.Instance.StartDialogue(BuildNoPizzaDialogue());
        }
        else
        {
            DialogueManager.Instance.StartDialogue(BuildDeliveryDialogue());
        }
    }

    DialogueNode BuildNoPizzaDialogue()
    {
        DialogueNode node = new DialogueNode
        {
            speakerName = "Amy",
            dialogueText = "Hi! I ordered a pizza. Could you bring it to me?",
            endsDialogue = false,
            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "I'll go pick it up.",
                    nextNode = new DialogueNode
                    {
                        speakerName = "Amy",
                        dialogueText = "Thank you! It's on the restaurant table.",
                        endsDialogue = true
                    }
                }
            }
        };

        return node;
    }

    DialogueNode BuildDeliveryDialogue()
    {
        bool hasCookie = cookiePickup != null && cookiePickup.HasCookie();

        DialogueNode endNode = new DialogueNode
        {
            speakerName = "Amy",
            dialogueText = "Thank you so much!",
            endsDialogue = false,
            autoContinue = true,
            autoContinueDelay = 1.5f,
            nextNode = new DialogueNode
            {
                endsDialogue = true
            }
        };

        DialogueNode cookieEndNode = new DialogueNode
        {
            speakerName = "Amy",
            dialogueText = "Wow! A fortune cookie too? That's so sweet! I'll give you an extra tip!",
            endsDialogue = false,
            autoContinue = true,
            autoContinueDelay = 2f,
            nextNode = new DialogueNode
            {
                endsDialogue = true
            }
        };

        DialogueNode cookieNode = new DialogueNode
        {
            speakerName = "Amy",
            dialogueText = "Oh wow! Thanks! I love fortune cookies.",
            endsDialogue = false,
            choices = new DialogueChoice[]
            {
            new DialogueChoice
                {
                    choiceText = "You're welcome!",
                    nextNode = cookieEndNode
                }
            }
        };

        DialogueNode givePizzaNode;

        if (hasCookie)
        {
            givePizzaNode = new DialogueNode
            {
                speakerName = "Amy",
                dialogueText = "Oh great! My pizza!",
                endsDialogue = false,
                choices = new DialogueChoice[]
                {
                new DialogueChoice
                {
                    choiceText = "Here is your pizza.",
                    nextNode = endNode
                },
                new DialogueChoice
                {
                    choiceText = "I also brought you a fortune cookie.",
                    nextNode = cookieNode
                }
                }
            };
        }
        else
        {
            givePizzaNode = new DialogueNode
            {
                speakerName = "Amy",
                dialogueText = "Oh great! My pizza!",
                endsDialogue = false,
                choices = new DialogueChoice[]
                {
                new DialogueChoice
                {
                    choiceText = "Here is your pizza.",
                    nextNode = endNode
                }
                }
            };
        }

        OrderManager.Instance.SubmitOrder();

        if (hasCookie && !bonusGiven)
        {
            GiveBonusTip(10);
            bonusGiven = true;
        }

        return givePizzaNode;
    }

    void GiveBonusTip(int amount)
    {
        DeliveryManager.Instance.AddEarnings(amount);

        EarningsUI earnings = FindObjectOfType<EarningsUI>();

        if (earnings != null)
            earnings.AddCurrentEarnings(amount);
    }
}