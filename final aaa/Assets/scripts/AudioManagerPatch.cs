using UnityEngine;

public class AudioManagerPatch : MonoBehaviour
{
    public static AudioManagerPatch Instance;
    public AudioSource sfxSource;
    public AudioClip pickupClip;   
    public AudioClip deliveryClip; 

    void Awake() { Instance = this; }

    public void PlayPickup() { if(pickupClip) sfxSource.PlayOneShot(pickupClip); }
    public void PlayDelivery() { if(deliveryClip) sfxSource.PlayOneShot(deliveryClip); }
}