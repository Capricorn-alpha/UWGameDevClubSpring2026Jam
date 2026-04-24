using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [Header("Grid Selection")]
    public RectTransform map;
    public Canvas canvas;

    public Color highlightColor = Color.green;
    public Vector2 borderSize = new Vector2(10, 10);
    private List<GameObject> selectedGroup = null;

    [Header("Grid Movement")]
    public float cellSize = 150f;
    public int mapSize = 5;
    private EnvironmentGroup group;
    private Dictionary<Sprite, List<GameObject>> environmentDictionary;

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

        HandleMovement();
    }

    void HandleMovement()
    {
        if (selectedGroup == null)
            return;

        Vector2 move = Vector2.zero;

        if (Keyboard.current.wKey.wasPressedThisFrame)
            move = Vector2.up;
        else if (Keyboard.current.sKey.wasPressedThisFrame)
            move = Vector2.down;
        else if (Keyboard.current.aKey.wasPressedThisFrame)
            move = Vector2.left;
        else if (Keyboard.current.dKey.wasPressedThisFrame)
            move = Vector2.right;

        if (move == Vector2.zero)
            return;

        Vector2 offset = move * cellSize;

        if (CanMoveGroup(offset))
        {
            foreach (GameObject obj in selectedGroup)
            {
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition += offset;
            }
        }
    }

    bool CanMoveGroup(Vector2 direction)
    {
        float halfMap = (mapSize - 1) * cellSize / 2f;

        foreach (GameObject obj in selectedGroup)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 currentPos = rect.anchoredPosition;

            if (direction == Vector2.right && currentPos.x >= halfMap)
                return false;

            if (direction == Vector2.left && currentPos.x <= -halfMap)
                return false;

            if (direction == Vector2.up && currentPos.y >= halfMap)
                return false;

            if (direction == Vector2.down && currentPos.y <= -halfMap)
                return false;
        }

        return true;
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