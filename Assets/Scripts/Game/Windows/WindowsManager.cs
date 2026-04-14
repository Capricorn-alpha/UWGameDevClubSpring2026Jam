using FrameWork.Event;
using Events;

[System.Serializable]
public class Window
{
    public SoftwareType type;
    public GameObject prefab;
    public string name;
}

public class WindowsManager : Singleton<WindowsManager>, IEventReceiver<SoftwareButtonClickedEvent>
{
    [SerializeField] private RectTransform windowParent;
    [SerializeField] private List<Window> windows = new();

    private readonly Dictionary<SoftwareType, GameObject> _prefabByType = new();
    private readonly Dictionary<SoftwareType, GameObject> _openByType = new();

    protected override void Awake()
    {
        base.Awake();
        foreach(var window in windows){
            _prefabByType[window.type] = window.prefab;
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe<SoftwareButtonClickedEvent>(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SoftwareButtonClickedEvent>(this);
    }

    public void OnEvent(SoftwareButtonClickedEvent e)
    {
        OpenWindow(e.type);
    }

    public void OpenWindow(SoftwareType type)
    {
        if(!_prefabByType.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogError($"Window prefab not found for type: {type}");
            return;
        }

        if(_openByType.TryGetValue(type, out GameObject existingWindow) && existingWindow != null)
        {
            existingWindow.SetActive(true);
            return;
        }
        
        Transform parent = windowParent != null ? windowParent : transform;
        GameObject instance = Instantiate(prefab, parent);
        _openByType[type] = instance;
    }
}
