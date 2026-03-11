using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [Header(" Start UI")]
    public GameObject startUI;

    
    public void PlayGame()
    {
        
        if (startUI != null)
        {
            startUI.SetActive(false);
        }
        
        
    }
}
