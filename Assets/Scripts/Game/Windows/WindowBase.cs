using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Base behaviour for a UI window. Drag the assigned handle (e.g. title bar) to move the window.
/// </summary>
public class WindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private Button closeButton;

    [SerializeField] private RectTransform dragHandle;

    private RectTransform _windowRect;

    private void Awake()
    {
        _windowRect = (RectTransform)transform;

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        if (dragHandle != null && dragHandle != _windowRect)
        {
            var forwarder = dragHandle.GetComponent<WindowDragForwarder>();
            if (forwarder == null)
                forwarder = dragHandle.gameObject.AddComponent<WindowDragForwarder>();
            forwarder.Bind(this);
        }
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        _windowRect.anchoredPosition += (Vector2)eventData.delta;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

/// <summary>
/// Forwards drag from a child handle to on the window root.
/// Unity sends drag events to the raycast target under the pointer, not to the parent.
/// </summary>
public sealed class WindowDragForwarder : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private WindowBase _window;

    public void Bind(WindowBase window) => _window = window;

    public void OnBeginDrag(PointerEventData eventData) => _window.OnBeginDrag(eventData);

    public void OnDrag(PointerEventData eventData) => _window.OnDrag(eventData);
}
