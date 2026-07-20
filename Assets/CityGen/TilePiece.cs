using UnityEngine;

public enum Dir { North, East, South, West }
public enum TileType { Road, Building, Park, Empty }
public enum EdgeType { Empty = 0, Road = 1, Building = 2 }


[CreateAssetMenu(fileName = "TilePiece", menuName = "CityGen/TilePiece")]
public class TilePiece : ScriptableObject
{
    public GameObject prefab;

    // Used by CityGenerator to name spawned GameObjects (e.g. "RoadStraight_3_5").
    // Defaults to the asset's own name if left blank.
    public string pieceName;

    public string edgeNums;
    public EdgeType[] edges = new EdgeType[4];

    void OnValidate()
    {
        if (edges == null || edges.Length != 4)
        {
            Debug.LogError($"TilePiece '{name}': edges array must have exactly 4 elements (N, E, S, W).");
        }

        if (string.IsNullOrEmpty(pieceName))
        {
            pieceName = name;
        }
    }

    public int allowedRotations = 4;

    // Relative likelihood this piece gets picked over others when multiple are valid.
    // Used by CityGenerator's WeightedShuffle.
    [Range(0.01f, 100f)]
    public float weight = 1f;

    public EdgeType GetEdge(Dir dir, int rotationSteps)
    {
        int rotatedIndex = ((int)dir - rotationSteps + 4) % 4;
        return edges[rotatedIndex];
    }
}