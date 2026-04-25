using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image tabImage;
    private TabManager manager;
    private int levelIndex;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        if (tabImage == null)
            tabImage = GetComponent<Image>();
    }

    public void Initialize(TabManager tabManager, int index)
    {
        manager = tabManager;
        levelIndex = index;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        manager.SelectTab(this);
    }

    public void SetSprite(Sprite sprite)
    {
        if (tabImage != null)
            tabImage.sprite = sprite;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }
}