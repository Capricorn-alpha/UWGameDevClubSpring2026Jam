using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base behaviour for a UI window. Drag is not implemented here yet.
/// </summary>
public class WindowBase : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
