using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public RectTransform map;
    public Canvas canvas;

    public Color highlightColor = Color.green;
    public Vector2 borderSize = new Vector2(10, 10);

    private EnvironmentGroup group;
    private Dictionary<Sprite, List<GameObject>> environmentDictionary;

    private List<GameObject> selectedGroup = null;

    void Start()
    {
        group = GetComponent<EnvironmentGroup>();
        environmentDictionary = group.GetGroup(map);
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectClick();
        }
    }

    void DetectClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Camera cam = null;
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            cam = canvas.worldCamera;
        }

        foreach (var pair in environmentDictionary)
        {
            Sprite keySprite = pair.Key;

            // Ignore map background
            if (keySprite == null || keySprite.name == "Map")
                continue;

            foreach (GameObject obj in pair.Value)
            {
                RectTransform rect = obj.GetComponent<RectTransform>();

                if (rect != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos, cam))
                {
                    // If clicking selected group again, deselect
                    if (selectedGroup == pair.Value)
                    {
                        ClearSelection();
                    }
                    else
                    {
                        SelectGroup(pair.Value);
                    }

                    return;
                }
            }
        }
    }

    void SelectGroup(List<GameObject> groupToSelect)
    {
        ClearSelection();

        selectedGroup = groupToSelect;

        foreach (GameObject environment in selectedGroup)
        {
            Transform bg = environment.transform.Find("BG");

            if (bg == null)
                continue;

            Outline outline = bg.GetComponent<Outline>();

            if (outline == null)
                outline = bg.gameObject.AddComponent<Outline>();

            outline.effectColor = highlightColor;
            outline.effectDistance = borderSize;
        }
    }

    void ClearSelection()
    {
        if (selectedGroup == null)
            return;

        foreach (GameObject environment in selectedGroup)
        {
            Transform bg = environment.transform.Find("BG");

            if (bg == null)
                continue;

            Outline outline = bg.GetComponent<Outline>();

            if (outline != null)
                Destroy(outline);
        }

        selectedGroup = null;
    }
}