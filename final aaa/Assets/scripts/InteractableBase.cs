using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 5f;
    public GameObject interactionPrompt;

   
    public abstract void Interact();

   
    protected bool IsPlayerInRange()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;
    }

    protected virtual void Update()
    {
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.I))
        {
            Interact();
        }
    }
}