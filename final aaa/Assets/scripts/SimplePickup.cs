using UnityEngine;

public class SimplePickup : MonoBehaviour
{
    public AudioClip pickupSound; 

    
    public void TriggerPickup() {
        if (AudioManagerPatch.Instance != null && pickupSound != null) {
            
            AudioManagerPatch.Instance.DirectPlay(pickupSound);
        }
        
        
        this.gameObject.SetActive(false); 
    }
}