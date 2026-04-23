using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image tabImage;
    private TabManager manager;

    public void Initialize(TabManager tabManager)
    {
        manager = tabManager;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        manager.SelectTab(this);
    }

    public void SetSprite(Sprite sprite)
    {
        tabImage.sprite = sprite;
    }
}
