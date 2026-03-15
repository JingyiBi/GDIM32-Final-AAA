using UnityEngine;

public class AudioManagerPatch : MonoBehaviour
{
    public static AudioManagerPatch Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource; 
    public AudioSource sfxSource; 

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; 
    public AudioClip pickupClip;   
    public AudioClip moneyClip;    

    void Awake() 
    { 
        Instance = this; 
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }

    
    public void PlayPickup() 
    { 
        if(pickupClip && sfxSource) sfxSource.PlayOneShot(pickupClip); 
    }

    
    public void PlayMoney() 
    { 
        if(moneyClip && sfxSource) sfxSource.PlayOneShot(moneyClip); 
    }
}