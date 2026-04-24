using UnityEngine;

[CreateAssetMenu(fileName = "NewEnvironment", menuName = "Map/Environments")]
public class Environment : ScriptableObject
{
    public string name;
    public Sprite map;
    public int score;
}

public enum EnvironmentType
{
    water = 0,
    grass = 1,
    rock = 2,
    waterGrass = 3,
    waterRock = 4,
    grassRock = 5,
    waterGrassRock = 6
}