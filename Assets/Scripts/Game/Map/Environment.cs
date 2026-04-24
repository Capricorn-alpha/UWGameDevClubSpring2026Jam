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
    rock,
    grass,
    water,
    waterGrass,
    rockWater,
    rockGrass,
    waterRockGrass
}
