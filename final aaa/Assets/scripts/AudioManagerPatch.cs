using UnityEngine;

public class AudioManagerPatch : MonoBehaviour
{
    public static AudioManagerPatch Instance;
    public AudioSource sfxSource; 

    void Awake() { 
        Instance = this; 
        
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }

    
    public void DirectPlay(AudioClip clip) {
        if(clip != null && sfxSource != null) {
            sfxSource.PlayOneShot(clip);
        }
    }
}