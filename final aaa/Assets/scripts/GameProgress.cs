using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    [Header("Quest Flags")]
    public bool antonDialogueDone = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}