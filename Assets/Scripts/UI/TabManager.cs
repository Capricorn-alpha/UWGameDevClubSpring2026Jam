using UnityEngine;

public class TabManager : MonoBehaviour
{
    private Sprite selectedSprite;
    private Sprite unselectedSprite;

    private Tab[] tabs;
    private Tab currentSelectedTab;

    [Header("Levels")]
    public LevelData[] levels; // Assign your 5 LevelData ScriptableObjects here
    public MapManager mapManager; // Drag the VsCodeWindow (root) GameObject here

    private void Awake()
    {
        selectedSprite = Resources.Load<Sprite>("Screen/Tab1");
        unselectedSprite = Resources.Load<Sprite>("Screen/Tab2");

        tabs = GetComponentsInChildren<Tab>();
    }

    private void Start()
    {
        for (int i = 0; i < tabs.Length && i < levels.Length; i++)
        {
            tabs[i].Initialize(this, i);
            tabs[i].SetSprite(unselectedSprite);
        }

        if (tabs.Length > 0)
        {
            SelectTab(tabs[0]);
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

        int levelIndex = clickedTab.GetLevelIndex();
        if (levels != null && levelIndex < levels.Length && levels[levelIndex] != null)
        {
            mapManager.LoadLevel(levels[levelIndex]);
        }
    }
}