using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text npcNameText; 
    public TMP_Text dialogueContentText;
    public Button option1Btn;
    public Button option2Btn;
    public Button option3Btn;

    public static DialogueSystem Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowDialogue(string npcName, string dialogue, string option1, string option2, string option3 = "")
    {
        gameObject.SetActive(true);
        npcNameText.text = npcName;
        dialogueContentText.text = dialogue;
        option1Btn.GetComponentInChildren<TMP_Text>().text = option1;
        option2Btn.GetComponentInChildren<TMP_Text>().text = option2;

        if (!string.IsNullOrEmpty(option3))
        {
            option3Btn.gameObject.SetActive(true);
            option3Btn.GetComponentInChildren<TMP_Text>().text = option3;
        }
        else
        {
            option3Btn.gameObject.SetActive(false);
        }

        option1Btn.onClick.RemoveAllListeners();
        option1Btn.onClick.AddListener(() =>
        {
            Debug.Log("选了选项1：接取订单");
            gameObject.SetActive(false); 
        });

        option2Btn.onClick.RemoveAllListeners();
        option2Btn.onClick.AddListener(() =>
        {
            Debug.Log("选了选项2：拒绝");
            gameObject.SetActive(false);
        });

        option3Btn.onClick.RemoveAllListeners();
        option3Btn.onClick.AddListener(() =>
        {
            Debug.Log("选了选项3：查看收益");
        });
    }
}