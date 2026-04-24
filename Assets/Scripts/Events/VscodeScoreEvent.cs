using UnityEngine;
namespace Events;

public readonly struct VscodeScoreEvent : MonoBehaviour
{
    private readonly int waterScore;
    public int water;
    private readonly int grassScore;
    public int grass;
    private readonly int rockScore;
    public int rock;
    private readonly int waterGrassScore;
    public int waterGrass;
    private readonly int waterRockScore;
    public int waterRock;
    private readonly int grassRockScore;
    public int grassRock;
    private readonly int waterGrassRockScore;
    public int waterGrassRock;

    public VscodeScoreEvent(int water, int grass, int rock, 
        int waterGrass, int waterRock, int grassRock, int waterGrassRock)
    {
        this.waterScore = waterScore * water;
        this.grassScore = grassScore * grass;
        this.rockScore = rockScore * rock;
        this.waterGrassScore = waterGrassScore * waterGrass;
        this.waterRockScore = waterRockScore * waterRock;
        this.grassRockScore = grassRockScore * grassRock;
        this.waterGrassRockScore = waterGrassRockScore * waterGrassRock;
    }
}
