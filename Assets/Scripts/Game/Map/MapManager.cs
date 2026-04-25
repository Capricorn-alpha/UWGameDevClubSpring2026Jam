using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public LevelData currentLevel;
    public Environment[] environments;
    public Sprite emptyTileSprite;

    [Header("UI Map Settings")]
    public GameObject tilePrefab;
    public Transform gridParent;

    [Header("UI Map References")]
    private Image[,] tileBackgroundImages; 
    private Image[,] tilePathImages;

    [Header("Path Sprites")]
    public Sprite pathVertical;
    public Sprite pathHorizontal;
    public Sprite pathCornerTopRight;
    public Sprite pathCornerTopLeft;
    public Sprite pathCornerBottomRight;
    public Sprite pathCornerBottomLeft;
    public Sprite pathStartEnd;

    private int[,] map;
    
    public EnvironmentGroup environmentGroup;

    void Start()
    {
        LoadLevel(currentLevel);
    }

    void LoadLevel(LevelData currentLevel)
    {
        map = new int[currentLevel.width, currentLevel.height];

        tileBackgroundImages = new Image[currentLevel.width, currentLevel.height];
        tilePathImages = new Image[currentLevel.width, currentLevel.height];

        for(int y = 0; y < currentLevel.height; y++)
        {
            for(int x = 0; x < currentLevel.width; x++)
            {
                map[x, y] = currentLevel.initialMapData[x + y * currentLevel.width];

                GameObject newTile = Instantiate(tilePrefab, gridParent);
                newTile.name = $"Tile_{x}_{y}";

                tileBackgroundImages[x, y] = newTile.transform.Find("BG").GetComponent<Image>();
                tilePathImages[x, y] = newTile.transform.Find("Path").GetComponent<Image>();
            }
        }
        UpdateMapUI();
    }

    void UpdateMapUI()
    {
        for (int x = 0; x < currentLevel.width; x++)
        {
            for (int y = 0; y < currentLevel.height; y++)
            {
                int envType = map[x, y];
                Image bgImage = tileBackgroundImages[x, y];

                if (envType == -1)
                {
                    bgImage.sprite = emptyTileSprite;
                }
                else
                {
                    bgImage.sprite = environments[envType].map;
                }
            }
        }

        UpdatePathVisuals();
    }

    void UpdatePathVisuals()
    {
        for (int x = 0; x < currentLevel.width; x++)
        {
            for (int y = 0; y < currentLevel.height; y++)
            {
                tilePathImages[x, y].sprite = null;
                tilePathImages[x, y].color = new Color(1, 1, 1, 0);
            }
        }

        if (currentLevel.pathCoordinates == null || currentLevel.pathCoordinates.Length == 0)
            return;

        Vector2Int[] path = currentLevel.pathCoordinates;

        for (int i = 0; i < path.Length; i++)
        {
            Vector2Int current = path[i];

            Vector2Int prev = (i > 0) ? path[i - 1] : current;
            Vector2Int next = (i < path.Length - 1) ? path[i + 1] : current;

            Image pathOverlay = tilePathImages[current.x, current.y];
            pathOverlay.color = new Color(1, 1, 1, 1);

            bool down = (prev.y > current.y) || (next.y > current.y);
            bool up = (prev.y < current.y) || (next.y < current.y);
            bool right = (prev.x > current.x) || (next.x > current.x);
            bool left = (prev.x < current.x) || (next.x < current.x);

            if (up && down)
            {
                pathOverlay.sprite = pathVertical;
            }
            else if (left && right)
            {
                pathOverlay.sprite = pathHorizontal;
            }
            else if (up && right)
            {
                pathOverlay.sprite = pathCornerTopRight;
            }
            else if (up && left)
            {
                pathOverlay.sprite = pathCornerTopLeft;
            }
            else if (down && right)
            {
                pathOverlay.sprite = pathCornerBottomRight;
            }
            else if (down && left)
            {
                pathOverlay.sprite = pathCornerBottomLeft;
            }
            else if (up || down) 
            {
                pathOverlay.sprite = pathVertical;
            }
            else if (left || right) 
            {
                pathOverlay.sprite = pathHorizontal;
            }
        }
    }


    // [Header("Grid Selection")]
    // public RectTransform map;
    // public Canvas canvas;

    // public Color highlightColor = Color.green;
    // public Vector2 borderSize = new Vector2(10, 10);
    // private List<GameObject> selectedGroup = null;

    // [Header("Grid Movement")]
    // public float cellSize = 150f;
    // public int mapSize = 5;
    // private EnvironmentGroup group;
    // private Dictionary<Sprite, List<GameObject>> environmentDictionary;

    // void Start()
    // {
    //     group = GetComponent<EnvironmentGroup>();
    //     environmentDictionary = group.GetGroup(map);
    // }

    // void Update()
    // {
    //     if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    //     {
    //         DetectClick();
    //     }

    //     HandleMovement();
    // }

    // void HandleMovement()
    // {
    //     if (selectedGroup == null)
    //         return;

    //     Vector2 move = Vector2.zero;

    //     if (Keyboard.current.wKey.wasPressedThisFrame)
    //         move = Vector2.up;
    //     else if (Keyboard.current.sKey.wasPressedThisFrame)
    //         move = Vector2.down;
    //     else if (Keyboard.current.aKey.wasPressedThisFrame)
    //         move = Vector2.left;
    //     else if (Keyboard.current.dKey.wasPressedThisFrame)
    //         move = Vector2.right;

    //     if (move == Vector2.zero)
    //         return;

    //     Vector2 offset = move * cellSize;

    //     if (CanMoveGroup(offset))
    //     {
    //         foreach (GameObject obj in selectedGroup)
    //         {
    //             RectTransform rect = obj.GetComponent<RectTransform>();
    //             rect.anchoredPosition += offset;
    //         }
    //     }
    // }

    // bool CanMoveGroup(Vector2 direction)
    // {
    //     float halfMap = (mapSize - 1) * cellSize / 2f;

    //     foreach (GameObject obj in selectedGroup)
    //     {
    //         RectTransform rect = obj.GetComponent<RectTransform>();
    //         Vector2 currentPos = rect.anchoredPosition;

    //         if (direction == Vector2.right && currentPos.x >= halfMap)
    //             return false;

    //         if (direction == Vector2.left && currentPos.x <= -halfMap)
    //             return false;

    //         if (direction == Vector2.up && currentPos.y >= halfMap)
    //             return false;

    //         if (direction == Vector2.down && currentPos.y <= -halfMap)
    //             return false;
    //     }

    //     return true;
    // }

    // void DetectClick()
    // {
    //     Vector2 mousePos = Mouse.current.position.ReadValue();

    //     Camera cam = null;
    //     if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
    //     {
    //         cam = canvas.worldCamera;
    //     }

    //     foreach (var pair in environmentDictionary)
    //     {
    //         Sprite keySprite = pair.Key;

    //         // Ignore map background
    //         if (keySprite == null || keySprite.name == "Map")
    //             continue;

    //         foreach (GameObject obj in pair.Value)
    //         {
    //             RectTransform rect = obj.GetComponent<RectTransform>();

    //             if (rect != null &&
    //                 RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos, cam))
    //             {
    //                 // If clicking selected group again, deselect
    //                 if (selectedGroup == pair.Value)
    //                 {
    //                     ClearSelection();
    //                 }
    //                 else
    //                 {
    //                     SelectGroup(pair.Value);
    //                 }

    //                 return;
    //             }
    //         }
    //     }
    // }

    // void SelectGroup(List<GameObject> groupToSelect)
    // {
    //     ClearSelection();

    //     selectedGroup = groupToSelect;

    //     foreach (GameObject environment in selectedGroup)
    //     {
    //         Transform bg = environment.transform.Find("BG");

    //         if (bg == null)
    //             continue;

    //         Outline outline = bg.GetComponent<Outline>();

    //         if (outline == null)
    //             outline = bg.gameObject.AddComponent<Outline>();

    //         outline.effectColor = highlightColor;
    //         outline.effectDistance = borderSize;
    //     }
    // }

    // void ClearSelection()
    // {
    //     if (selectedGroup == null)
    //         return;

    //     foreach (GameObject environment in selectedGroup)
    //     {
    //         Transform bg = environment.transform.Find("BG");

    //         if (bg == null)
    //             continue;

    //         Outline outline = bg.GetComponent<Outline>();

    //         if (outline != null)
    //             Destroy(outline);
    //     }

    //     selectedGroup = null;
    // }
}