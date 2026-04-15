using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FrameWork.Event;
using Events;

public class PostUI : MonoBehaviour
{
    [Header("Comment")]
    [SerializeField] private Button commentButton;
    [SerializeField] private GameObject popupMenuPrefab;

    [SerializeField] private bool hidePopupMenu = true;
    [SerializeField] private bool toggleOnClick = true;

    [Header("Reply")]
    [SerializeField] private Button[] popupMenuButtons = new Button[3];
    [SerializeField] private GameObject replyPrefab;
    [SerializeField] private bool hideReplyPrefab = true;
    [SerializeField] private TMP_Text replyContentText;

    private string replyTextFor1 = "Thanks for your feedback! We'll look into it.";
    private string replyTextFor2 = "It's NOT a bug, it's a feature.";
    private string replyTextFor3 = "YOU ALL DON'T UNDERSTAND!";

    // protected void Awake()
    // {
    //     if(commentButton != null)
    //     {
    //         commentButton.onClick.AddListener(OnCommentButtonClicked);
    //     }
    // }

    private void Start()
    {
        if (hidePopupMenu && popupMenuPrefab != null)
        {
            popupMenuPrefab.SetActive(false);
        }

        if (hideReplyPrefab && replyPrefab != null)
        {
            replyPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (commentButton != null)
        {
            commentButton.onClick.AddListener(OnCommentButtonClicked);
        }

        for (int i = 0; i < popupMenuButtons.Length; i++)
        {   
            if (popupMenuButtons[i] == null) continue;

            int index = i;
            popupMenuButtons[index].onClick.AddListener(() => OnPopupMenuButtonClicked(index));
        }
    }

    private void OnDisable()
    {
        if (commentButton != null)
            commentButton.onClick.RemoveListener(OnCommentButtonClicked);
    }

    private void OnCommentButtonClicked()
    {

        if (popupMenuPrefab == null) return;

        bool canShow = toggleOnClick ? !popupMenuPrefab.activeSelf : true;
        popupMenuPrefab.SetActive(canShow);

        if (canShow)
            popupMenuPrefab.transform.SetAsLastSibling();
    }

    private void OnPopupMenuButtonClicked(int index)
    {
        HidePopupMenu();

        if (replyPrefab != null)
            replyPrefab.SetActive(true);

        replyContentText.text = ApplyReplyText(index);
        
        EventBus.Publish(new AfterReplyCommentEvent(index));
    }

    private string ApplyReplyText(int index)
    {
        if (replyContentText == null) return "";

        string body = "";
        switch (index)
        {
            case 0:
                body = replyTextFor1;
                break;
            case 1:
                body = replyTextFor2;
                break;
            case 2:
                body = replyTextFor3;
                break;
        }

        return body;
    }

    public void ShowPopupMenu()
    {
        if (popupMenuPrefab == null) return;
        popupMenuPrefab.SetActive(true);
        popupMenuPrefab.transform.SetAsLastSibling();
    }

    public void HidePopupMenu()
    {
        if (popupMenuPrefab == null) return;
        popupMenuPrefab.SetActive(false);
    }
}
