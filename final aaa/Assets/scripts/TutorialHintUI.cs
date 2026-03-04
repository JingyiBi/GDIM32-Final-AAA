using UnityEngine;

public class TutorialHintUI : MonoBehaviour
{
    public static TutorialHintUI Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Invoke("HideHint", 5f);
    }

    public void HideHint()
    {
        gameObject.SetActive(false);
    }
}