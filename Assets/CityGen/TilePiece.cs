using UnityEngine;

/// <summary>
/// Direction enum representing the four cardinal directions.🧭🗺️👆👉👇👈 Used for indexing into the edges array of a TilePiece.
/// </summary>
public enum Dir { North, East, South, West }

/// <summary>
/// Category of a tile piece.🧩🤔💭 Used to determine which pieces can be conencted by edge types. 
public enum TileType { Empty, Road, Building }

/// <summary>
/// Edge type of a tile piece. 🛣️🛣️🚗🛣️🛣️ Used to determine how pieces conenct with each other. 
/// Ex: 0 == 0, 1 == 1, 2==2 means those two edges of those two tiles can connect. 
public enum EdgeType { Empty = 0, Road = 1, Building = 2, Sidewalk = 3 }

/// <summary>
/// TilePiece class represents single tile piece in the city generation 🤔💭🧩🧩🧩🧩➡️🏬. The [CreateAssetMenu] attribute
/// allows you to create instances of this class as assets in the Unity Editor. This way it is now possible to make new
///  TilePiece assets through the Unity Editor by right clicking in the Project window and going Create -> CityGen (name of my folder that holds all the scripts+assets) 
/// -> TilePiece. Then you just  deadass drag in ALL the info for the tilepiece into the inspector  📑✍️🔥🔥
/// </summary>
[CreateAssetMenu(fileName = "TilePiece", menuName = "CityGen/TilePiece")]
public class TilePiece : ScriptableObject
{
    [Header("Tile Piece Info")]
    [Tooltip("Prefab representing this tile piece.")]
    public GameObject prefab;
    [Tooltip("Name of the tile piece. If left empty, will default to the name of the prefab.")]
    public string pieceName;
    [Tooltip("Type of the tile piece.")]
    public TileType category;
    [Tooltip("String representing the edge types for each direction (N=0, E=1, S=2, W=3). This is mainly used for helping me visualize the edge types in the inspector. It is not used in the code.")]
    public string edgeNums;
    [Tooltip("The edge types for each direction in N, W, S, E order.")]
    public EdgeType[] edges = new EdgeType[4];
    [Tooltip("Used to calculate weight for the tile piece when generating the city. Higher weight means the piece is more likely to be chosen.")]
    [Range(0.01f, 100f)]
    public float weight = 1f;

    /// <summary>
    /// OnValidate called when the user interacts with an Inspector in the Editor. 
    /// </summary>
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

    /// <summary>
    /// Returns the edge type for a given direction after applying rotation.
    /// </summary>
    public EdgeType GetEdge(Dir dir, int rotationSteps)
    {
        int rotatedIndex = ((int)dir - rotationSteps + 4) % 4;
        return edges[rotatedIndex];
    }
}