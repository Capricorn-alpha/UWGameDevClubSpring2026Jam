using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Events;

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
    private int selectedEnvType = -1;
    private Stack<(int[,] mapSnapshot, List<Vector2Int> selectedSnapshot, int selectedEnvSnapshot)> moveHistory 
        = new Stack<(int[,], List<Vector2Int>, int)>();
    

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

        if (currentlySelectedTiles.Count > 0 && selectedEnvType != -1)
        {
            Vector2 moveDirection = Vector2.zero;
            if (Keyboard.current.sKey.wasPressedThisFrame) moveDirection = Vector2.up;
            else if (Keyboard.current.wKey.wasPressedThisFrame) moveDirection = Vector2.down;
            else if (Keyboard.current.aKey.wasPressedThisFrame) moveDirection = Vector2.left;
            else if (Keyboard.current.dKey.wasPressedThisFrame) moveDirection = Vector2.right;
            
            if (moveDirection != Vector2.zero)
                MoveSelectedGroup(moveDirection);
        }

        if (Keyboard.current.zKey.wasPressedThisFrame && (Keyboard.current.ctrlKey.isPressed || Keyboard.current.leftCtrlKey.isPressed))
        {
            Undo();
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

     void HandleTileSelection()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2Int? gridPosition = GetGridPositionFromMouse(mousePosition);
        
        if (gridPosition.HasValue)
        {
            int x = gridPosition.Value.x;
            int y = gridPosition.Value.y;
            
            if (x >= 0 && x < currentLevel.width && y >= 0 && y < currentLevel.height && map[x, y] != -1)
            {
                int clickedEnvType = map[x, y];
                selectedEnvType = clickedEnvType;
                List<Vector2Int> connectedTiles = GetConnectedTilesOfSameType(x, y, clickedEnvType);
                ClearSelection();
                HighlightTiles(connectedTiles);
                currentlySelectedTiles = connectedTiles;
            }
            else
            {
                ClearSelection();
            }
        }
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

    void MoveSelectedGroup(Vector2Int direction)
    {
        SaveState();
        // Calculate new positions
        List<Vector2Int> newPositions = new List<Vector2Int>();
        foreach (Vector2Int tile in currentlySelectedTiles)
        {
            Vector2Int newPos = new Vector2Int(tile.x + direction.x, tile.y + direction.y);
            
            if (newPos.x < 0 || newPos.x >= currentLevel.width || 
                newPos.y < 0 || newPos.y >= currentLevel.height)
                return;
            newPositions.Add(newPos);
        }
        
        // Store old values and collect overlapping types
        Dictionary<Vector2Int, HashSet<int>> overlappingTypes = new Dictionary<Vector2Int, HashSet<int>>();
        Dictionary<Vector2Int, int> originalTypes = new Dictionary<Vector2Int, int>();
        
        for (int i = 0; i < currentlySelectedTiles.Count; i++)
        {
            Vector2Int newPos = newPositions[i];
            int existingType = map[newPos.x, newPos.y];
            
            if (existingType != -1)
            {
                if (!overlappingTypes.ContainsKey(newPos))
                    overlappingTypes[newPos] = new HashSet<int>();
                overlappingTypes[newPos].Add(existingType);
                overlappingTypes[newPos].Add(selectedEnvType);
                originalTypes[newPos] = existingType;
            }
        }
        
        // Clear old positions (only if they're not overlapping with something)
        for (int i = 0; i < currentlySelectedTiles.Count; i++)
        {
            Vector2Int oldPos = currentlySelectedTiles[i];
            Vector2Int newPos = newPositions[i];
            
            // Only clear if this tile isn't overlapping with another moving tile
            bool isOverlapping = false;
            for (int j = 0; j < newPositions.Count; j++)
            {
                if (i != j && newPositions[j] == oldPos)
                {
                    isOverlapping = true;
                    break;
                }
            }
            
            if (!isOverlapping && !overlappingTypes.ContainsKey(oldPos))
            {
                map[oldPos.x, oldPos.y] = -1;
            }
        }
        
        // Apply new positions with possible combinations
        for (int i = 0; i < currentlySelectedTiles.Count; i++)
        {
            Vector2Int newPos = newPositions[i];
            
            if (overlappingTypes.ContainsKey(newPos))
            {
                int combinedType = GetCombinedType(overlappingTypes[newPos]);
                if (combinedType != -1)
                    map[newPos.x, newPos.y] = combinedType;
            }
            else if (map[newPos.x, newPos.y] == -1)
            {
                map[newPos.x, newPos.y] = selectedEnvType;
            }
            // else keep original index (do nothing)
        }
        
        // Update selection and UI
        currentlySelectedTiles = newPositions;
        ClearSelection();
        HighlightTiles(currentlySelectedTiles);
        UpdateMapUI();
    }

    void MoveSelectedGroup(Vector2 direction)
    {
        MoveSelectedGroup(new Vector2Int((int)direction.x, (int)direction.y));
    }

    int GetCombinedType(HashSet<int> environmentTypes)
    {
        if (environmentTypes.Contains(0) && environmentTypes.Contains(1) && environmentTypes.Contains(2))
            return 6;
        if (environmentTypes.Contains(0) && environmentTypes.Contains(1))
            return 3;
        if (environmentTypes.Contains(0) && environmentTypes.Contains(2))
            return 4;
        if (environmentTypes.Contains(1) && environmentTypes.Contains(2))
            return 5;
        return -1;
    }

    void SaveState()
    {
        // Copy current map
        int[,] mapCopy = new int[currentLevel.width, currentLevel.height];
        for (int x = 0; x < currentLevel.width; x++)
            for (int y = 0; y < currentLevel.height; y++)
                mapCopy[x, y] = map[x, y];
        
        // Copy selected tiles
        List<Vector2Int> selectedCopy = new List<Vector2Int>(currentlySelectedTiles);
        
        moveHistory.Push((mapCopy, selectedCopy, selectedEnvType));
    }

    void Undo()
    {
        if (moveHistory.Count == 0) return;
        
        var previousState = moveHistory.Pop();
        
        // Restore map
        for (int x = 0; x < currentLevel.width; x++)
            for (int y = 0; y < currentLevel.height; y++)
                map[x, y] = previousState.mapSnapshot[x, y];
        
        // Restore selection
        ClearSelection();
        currentlySelectedTiles = previousState.selectedSnapshot;
        selectedEnvType = previousState.selectedEnvSnapshot;
        HighlightTiles(currentlySelectedTiles);
        
        UpdateMapUI();
    }


}