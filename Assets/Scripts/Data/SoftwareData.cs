using UnityEngine;

[CreateAssetMenu(menuName = "Software/SoftwareData", fileName = "New SoftwareData")]
public class SoftwareData : ScriptableObject
{
    public SoftwareType softwareType;
    public Sprite icon;
    public string displayName;
}
