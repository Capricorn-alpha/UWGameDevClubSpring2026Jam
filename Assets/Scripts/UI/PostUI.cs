using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PostUI : Singleton<PostUI>
{
    [SerializeField] private Button commentButton;
    [SerializeField] private GameObject popupMenuPrefab;

    [SerializeField] private bool hidePopupMenu = true;
    [SerializeField] private bool toggleOnClick = true;

    protected override void Awake(){
        base.Awake();
        if(Instance != this) return;
    }

    private void Start()
    {
        if(hidePopupMenu && popupMenuPrefab != null
        ){
            popupMenuPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(commentButton != null)
        {
            commentButton.onClick.AddListener(OnCommentButtonClicked);
        }
    }

    private void OnDisable()
    {
        if(commentButton != null)
        {
            commentButton.onClick.RemoveListener(OnCommentButtonClicked);
        }
    }

    private void OnCommentButtonClicked()
    {
        if(popupMenuPrefab == null)
        {
            Debug.Log("OnCommentButtonClicked");
            return;
        }

        bool CanShow = toggleOnClick ? !popupMenuPrefab.activeSelf : true;
        popupMenuPrefab.SetActive(CanShow);

        if(CanShow){
            popupMenuPrefab.transform.SetAsLastSibling();
        }
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
