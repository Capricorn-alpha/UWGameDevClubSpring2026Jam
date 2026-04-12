using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<SoftwareData> softwareDatas;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonHolder;

    public IReadOnlyList<SoftwareData> SoftwareDatas => softwareDatas;

    private void Awake() => RebuildButtons();

    /// <summary>清空 holder 下已有子物体，并按列表重新生成按钮。</summary>
    public void RebuildButtons()
    {
        if (buttonPrefab == null || buttonHolder == null || softwareDatas == null)
            return;

        for (int i = buttonHolder.childCount - 1; i >= 0; i--)
            Destroy(buttonHolder.GetChild(i).gameObject);

        foreach (SoftwareData data in softwareDatas)
        {
            if (data == null)
                continue;

            GameObject instance = Instantiate(buttonPrefab, buttonHolder);
            var buttonUI = instance.GetComponent<ButtonUI>();
            if (buttonUI != null)
                buttonUI.ApplySoftwareData(data);
        }
    }
}
