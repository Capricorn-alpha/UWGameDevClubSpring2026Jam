using UnityEngine;

public class TabManager : MonoBehaviour
{
    private Sprite selectedSprite;
    private Sprite unselectedSprite;
    private int index = 0;

    private Tab[] tabs;
    private Tab currentSelectedTab;

    private void Awake()
    {
        selectedSprite = Resources.Load<Sprite>("Screen/Tab1");
        unselectedSprite = Resources.Load<Sprite>("Screen/Tab2");

        tabs = GetComponentsInChildren<Tab>();
    }

    private void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].Initialize(this);
            tabs[i].SetSprite(unselectedSprite);
        }

        if (tabs.Length > 0 && index >= 0 && index < tabs.Length)
        {
            SelectTab(tabs[index]);
        }
    }

    public void SelectTab(Tab clickedTab)
    {
        if (clickedTab == null) return;
        if (currentSelectedTab == clickedTab) return;

        if (currentSelectedTab != null)
        {
            currentSelectedTab.SetSprite(unselectedSprite);
        }

        clickedTab.SetSprite(selectedSprite);
        currentSelectedTab = clickedTab;
    }
}
