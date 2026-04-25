using UnityEngine;

[CreateAssetMenu(fileName = "Level_001", menuName = "Map/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Size")]
    public int width = 5;
    public int height = 5;

    [Header("Level Environments")]
    public int[] initialMapData;

    [Header("Path")]
    public Vector2Int[] pathCoordinates;
}
