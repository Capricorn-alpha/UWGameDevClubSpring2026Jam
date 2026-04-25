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
    private Outline[,] tileOutlines; // Store outline components

    [Header("Path Sprites")]
    public Sprite pathVertical;
    public Sprite pathHorizontal;
    public Sprite pathCornerTopRight;
    public Sprite pathCornerTopLeft;
    public Sprite pathCornerBottomRight;
    public Sprite pathCornerBottomLeft;

    [Header("Selection Settings")]
    public Color selectionOutlineColor = Color.red;
    public Color defaultOutlineColor = new Color(0f, 0f, 0f, 0f);
    private List<Vector2Int> currentlySelectedTiles = new List<Vector2Int>();

    private int[,] map;

    void Start()
    {
        LoadLevel(currentLevel);
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleTileSelection();
        }
    }

    void LoadLevel(LevelData currentLevel)
    {
        if (currentLevel == null)
        {
            Debug.LogError("CurrentLevel is null!");
            return;
        }

        map = new int[currentLevel.width, currentLevel.height];
        tileBackgroundImages = new Image[currentLevel.width, currentLevel.height];
        tilePathImages = new Image[currentLevel.width, currentLevel.height];
        tileOutlines = new Outline[currentLevel.width, currentLevel.height];

        for(int y = 0; y < currentLevel.height; y++)
        {
            for(int x = 0; x < currentLevel.width; x++)
            {
                map[x, y] = currentLevel.initialMapData[x + y * currentLevel.width];

                GameObject newTile = Instantiate(tilePrefab, gridParent);
                newTile.name = $"Tile_{x}_{y}";

                Transform bgTransform = newTile.transform.Find("BG");
                Transform pathTransform = newTile.transform.Find("Path");
                
                if (bgTransform != null)
                {
                    tileBackgroundImages[x, y] = bgTransform.GetComponent<Image>();
                    
                    // Get or add Outline component to BG
                    Outline outline = bgTransform.GetComponent<Outline>();
                    if (outline == null)
                    {
                        outline = bgTransform.gameObject.AddComponent<Outline>();
                    }
                    outline.enabled = false; // Start with outline disabled
                    outline.effectColor = selectionOutlineColor;
                    outline.effectDistance = new Vector2(3, 3);
                    tileOutlines[x, y] = outline;
                }
                else
                {
                    Debug.LogError($"BG transform not found on tile {x},{y}");
                }
                    
                if (pathTransform != null)
                {
                    tilePathImages[x, y] = pathTransform.GetComponent<Image>();
                }
                else
                {
                    Debug.LogError($"Path transform not found on tile {x},{y}");
                }
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
                
                if (bgImage == null) continue;

                if (envType == -1)
                {
                    bgImage.sprite = emptyTileSprite;
                }
                else
                {
                    if (envType < environments.Length && environments[envType] != null)
                        bgImage.sprite = environments[envType].map;
                    else
                        Debug.LogError($"Environment type {envType} not found!");
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
                if (tilePathImages[x, y] != null)
                {
                    tilePathImages[x, y].sprite = null;
                    tilePathImages[x, y].color = new Color(1, 1, 1, 0);
                }
            }
        }

        if (currentLevel.pathCoordinates == null || currentLevel.pathCoordinates.Length == 0)
            return;

        Vector2Int[] path = currentLevel.pathCoordinates;

        for (int i = 0; i < path.Length; i++)
        {
            Vector2Int current = path[i];
            
            // Check bounds
            if (current.x >= currentLevel.width || current.y >= currentLevel.height)
                continue;

            Vector2Int prev = (i > 0) ? path[i - 1] : current;
            Vector2Int next = (i < path.Length - 1) ? path[i + 1] : current;

            Image pathOverlay = tilePathImages[current.x, current.y];
            if (pathOverlay == null) continue;
            
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

    public void OnSubmitButtonClicked()
    {
        int[] environmentCounts = new int[7];

        if(currentLevel.pathCoordinates != null && currentLevel.pathCoordinates.Length > 0)
        {
            foreach(Vector2Int coord in currentLevel.pathCoordinates)
            {
                if(coord.x >= 0 && coord.x < currentLevel.width && 
                    coord.y >= 0 && coord.y < currentLevel.height)
                {
                    int envType = map[coord.x, coord.y];
                    
                    if(envType >= 0 && envType < environments.Length)
                    {
                        environmentCounts[envType]++;
                    }
                }
            }
        }

        Debug.Log($"结算完成! 水:{environmentCounts[0]}, 草:{environmentCounts[1]}, 石:{environmentCounts[2]}, 水草:{environmentCounts[3]}, 水石:{environmentCounts[4]}, 草石:{environmentCounts[5]}, 水草石:{environmentCounts[6]}");

        EventBus.Publish(new VscodeScoreEvent(environmentCounts));
    }


    Vector2Int? GetGridPositionFromMouse(Vector2 mousePosition)
    {
        RectTransform gridRect = gridParent as RectTransform;
        if (gridRect == null || gridParent.childCount == 0) return null;
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridRect, mousePosition, null, out localPoint);
        
        RectTransform firstTile = gridParent.GetChild(0) as RectTransform;
        float tileWidth = firstTile.rect.width;
        float tileHeight = firstTile.rect.height;
        
        int x = Mathf.FloorToInt((localPoint.x + gridRect.rect.width / 2) / tileWidth);
        int y = Mathf.FloorToInt((-localPoint.y + gridRect.rect.height / 2) / tileHeight);
        
        return (x >= 0 && x < currentLevel.width && y >= 0 && y < currentLevel.height) ? new Vector2Int(x, y) : null;
    }

    List<Vector2Int> GetConnectedTilesOfSameType(int startX, int startY, int targetEnvType)
    {
        List<Vector2Int> connectedTiles = new List<Vector2Int>();
        Queue<Vector2Int> tilesToCheck = new Queue<Vector2Int>();
        bool[,] visited = new bool[currentLevel.width, currentLevel.height];
        
        tilesToCheck.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;
        
        Vector2Int[] directions = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };
        
        while (tilesToCheck.Count > 0)
        {
            Vector2Int current = tilesToCheck.Dequeue();
            
            if (map[current.x, current.y] == targetEnvType)
            {
                connectedTiles.Add(current);
                
                foreach (Vector2Int dir in directions)
                {
                    int newX = current.x + dir.x;
                    int newY = current.y + dir.y;
                    
                    if (newX >= 0 && newX < currentLevel.width && newY >= 0 && newY < currentLevel.height && !visited[newX, newY])
                    {
                        visited[newX, newY] = true;
                        tilesToCheck.Enqueue(new Vector2Int(newX, newY));
                    }
                }
            }
        }
        return connectedTiles;
    }

    void HighlightTiles(List<Vector2Int> tiles)
    {
        foreach (Vector2Int tile in tiles)
        {
            if (tileOutlines[tile.x, tile.y] != null)
            {
                tileOutlines[tile.x, tile.y].enabled = true;
                tileOutlines[tile.x, tile.y].effectColor = selectionOutlineColor;
            }
        }
    }

    void ClearSelection()
    {
        foreach (Vector2Int tile in currentlySelectedTiles)
        {
            if (tileOutlines[tile.x, tile.y] != null)
                tileOutlines[tile.x, tile.y].enabled = false;
        }
        currentlySelectedTiles.Clear();
    }
}