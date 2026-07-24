using UnityEngine;

public enum Dir { North, East, South, West }
public enum TileType { Road, Building, Park, Empty }
public enum EdgeType { Empty = 0, Road = 1, Building = 2 , Sidewalk =3}


[CreateAssetMenu(fileName = "TilePiece", menuName = "CityGen/TilePiece")]
public class TilePiece : ScriptableObject
{
    [Tooltip("Prefab representing this tile piece.")]
    public GameObject prefab;
    [Tooltip("Name of the tile piece. If left empty, will default to the name of the prefab.")]
    public string pieceName;

    [Tooltip("Type of the tile piece.")]
    public TileType category;

    [Tooltip("String representing the edge types for each direction (N, E, S, W).")]
    public string edgeNums;
    public EdgeType[] edges = new EdgeType[4];

    void OnValidate()
    {
        if (edges == null || edges.Length != 4)
        {
            Debug.LogError("Edges array must have exactly 4 elements (N, E, S, W).");
        }

        if (string.IsNullOrEmpty(pieceName))
        {
            pieceName = name;
        }
    }

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