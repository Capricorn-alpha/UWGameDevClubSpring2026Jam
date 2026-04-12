using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private SoftwareType softwareType;
    [SerializeField] private Image iconImage;

    // Called by the Button's OnClick event (wire in the Inspector)
    public void OnButtonClicked()
    {
        Debug.Log($"Button clicked - SoftwareType: {softwareType}");
    }

    // Optional: public getter for other scripts
    public SoftwareType GetSoftwareType() => softwareType;

    // Optional: allow other code to set the type at runtime
    public void SetSoftwareType(SoftwareType type) => softwareType = type;

    public void ApplySoftwareData(SoftwareData data)
    {
        if (data == null)
            return;

        softwareType = data.softwareType;

        Image img = ResolveIconImage();
        if (img != null && data.icon != null)
            img.sprite = data.icon;
    }

    private Image ResolveIconImage()
    {
        if (iconImage != null)
            return iconImage;

        Transform icon = transform.Find("Icon");
        if (icon != null && icon.TryGetComponent(out Image found))
            return found;

        foreach (Image img in GetComponentsInChildren<Image>(true))
        {
            if (img.transform != transform)
                return img;
        }

        return GetComponent<Image>();
    }
}
