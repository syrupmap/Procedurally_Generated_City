using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// CityGenerator is a class that procedurally generates a city layout using the tile peices from TilePiece.cs. It creates a grid of tiles and ensures that the edges of the tiles match up. 
/// </summary>
public class CityGenerator : MonoBehaviour
{
    [Tooltip("List of tile pieces to use for city generation.")]
    public List<TilePiece> tilePieces;
    [Tooltip("Width of the city grid.")]
    public int gridWidth = 10;
    [Tooltip("Height of the city grid.")]
    public int gridHeight = 10;
    [Tooltip("Size of each tile in world units. This is used to position the tiles correctly in the scene. In blender my assets were 4x4x4 meters but in Unity they ended up becoming 16x16x16. ")]
    public float tileSize = 16;
    [Tooltip("Maximum number of attempts to place tiles before giving up. Not every city layout is possible, so this safegaaurd prevents infinite loops.")]
    public int maxAttempts = 1000;
    public int minPossibleRoad = 2;
    public int maxPossibleRoad = 6;

    /// <summary>
    /// Basically a data structure that holds a tile piece and its rotation. 
    /// </summary>
    private class PlacedTile
    {
        public TilePiece tilePiece;
        public int rotationSteps;
    }

    /// <summary>
    /// A 2D grid that marks true or false for whether a cell is forced to be a road. Added for blocking
    /// </summary>
    private bool[,] roadMask;

    /// <summary>
    /// A 2D grid that holds the placed tiles 
    /// </summary>
    private PlacedTile[,] grid;

    /// <summary>
    /// Number of attempts used to place tiles. Attempts stop when it reaches maxAttemps. 
    /// </summary>
    private int attemptsUsed;

    /// <summary>
    /// Little Unity guy that runs once when scene starts playing. Calls GenerateCity. 
    /// </summary>
    void Start()
    {
        GenerateCity();
    }

    /// <summary>
    /// Main controller for city generation. 
    ///     1) Begins by creating the road map. Converts the road lists to hashsets cause I learned in class that it helps with optimization. Marks every road cell in the boolean "road" 2D array. T for road, F for non-road
    ///     2) Delete the old city with DestroyImmediate. 
    ///     3) Create actual city grid that is full of null. Place and fill all roadTiles first 
    ///     4) Reset attemptsUsed = 0
    ///     5) Fill the remaining spaces and check for failure. If not, then city shall be instantiated.
    /// </summary>
    public void GenerateCity()
    {
        var verticalRoads = GenerateRoadLines(gridWidth, minPossibleRoad, maxPossibleRoad);
        var horizontalRoads = GenerateRoadLines(gridHeight, minPossibleRoad, maxPossibleRoad);
        roadMask = new bool[gridWidth, gridHeight];
        HashSet<int> vRoads = new(verticalRoads);
        HashSet<int> hRoads = new(horizontalRoads);
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                roadMask[x, y] = vRoads.Contains(x) || hRoads.Contains(y);
            }
        }
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        grid = new PlacedTile[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (!roadMask[x, y])
                {
                    continue;
                }
                var road = GetRoadTile(x, y);
                if (road.piece != null)
                {
                    grid[x, y] = new PlacedTile
                    {
                        tilePiece = road.piece,
                        rotationSteps = road.rotation
                    };
                }
            }
        }
        attemptsUsed = 0;
        bool success = TryFillGrid(0, 0);
        if (!success)
        {
            Debug.LogWarning("CityGenerator: could not find a fully valid layout");
        }
        InstantiateGrid();
    }

    /// <summary>
    /// RECURSIVE BACKTRACKING. Fill every non-road cell with a valid tile. IF a cell doesn't work then restart. 
    /// </summary>
    bool TryFillGrid(int x, int y)
    {
        if (y >= gridHeight)
        {
            return true;
        }
        int nextX = (x + 1);
        int nextY = y;
        if (nextX >= gridWidth)
        {
            nextX = 0;
            nextY = y + 1;
        }
        if (roadMask[x, y])
        {
            return TryFillGrid(nextX, nextY);
        }
        List<(TilePiece piece, int rotationSteps)> validTiles = GetValidTiles(x, y);
        WeightedShuffle(validTiles);
        foreach (var tiles in validTiles)
        {
            if (attemptsUsed++ > maxAttempts)
            {
                return false;
            }
            grid[x, y] = new PlacedTile { tilePiece = tiles.piece, rotationSteps = tiles.rotationSteps };
            if (TryFillGrid(nextX, nextY))
            {
                return true;
            }
            grid[x, y] = null;
        }
        return false;
    }

    /// <summary>
    /// Returns a list of valid tiles at x,y. In simple terms it Looks at neighbors, gets edge requirements, tests every possible tile and rotation, and then adds the tiles that fit to the list. 
    /// Since the our algorithm fills RightDown, we only check in the west and south directions cause everything up and left are already filled (starting from 0,0)
    /// (0,0) → (1,0) → (2,0) → (3,0)
    ///                   ⬇️
    ///                   ⬇️
    ///(0, 1) → (1,1) → (2,1) → (3,1)
    /// </summary>
    List<(TilePiece piece, int rotationSteps)> GetValidTiles(int x, int y)
    {
        var result = new List<(TilePiece piece, int rotationSteps)>(); //this is the empty answer list
        EdgeType? westNeighborEastSocket = null;  // '?' allows EdgeType to start null. Without it, you get "Cannot convert null to 'EdgeType' because it is a non-nullable value type" Error
        TileType? westNeighborCategory = null;
        if (x > 0 && grid[x - 1, y] != null)
        {
            westNeighborEastSocket = grid[x - 1, y].tilePiece.GetEdge(Dir.East, grid[x - 1, y].rotationSteps);
            westNeighborCategory = grid[x - 1, y].tilePiece.category;
        }
        EdgeType? southNeighborNorthSocket = null;
        TileType? southNeighborCategory = null;
        if (y > 0 && grid[x, y - 1] != null)
        {
            southNeighborNorthSocket = grid[x, y - 1].tilePiece.GetEdge(Dir.North, grid[x, y - 1].rotationSteps);
            southNeighborCategory = grid[x, y - 1].tilePiece.category;
        }

        foreach (var piece in tilePieces)
        {
            // if (result.Count == 0)
            // {
            //     Debug.LogError($"No valid tiles at ({x},{y})");
            // }
            if (piece.prefab == null) continue;
            int rotStep = 4 / Mathf.Max(1, 4);
            for (int r = 0; r < 4; r += rotStep)
            {
                if (!EdgeMatch(westNeighborEastSocket, westNeighborCategory, piece.GetEdge(Dir.West, r), piece.category)) continue;
                if (!EdgeMatch(southNeighborNorthSocket, southNeighborCategory, piece.GetEdge(Dir.South, r), piece.category)) continue;

                result.Add((piece, r));
            }
        }
        return result;
    }

    /// <summary>
    /// The list is shuffled randomly and then then all tiles are given a random weight from 0 to their piece weight.
    /// </summary>
    void WeightedShuffle(List<(TilePiece piece, int rotationSteps)> list)
    {
        for (int i = 0; i < list.Count; i++) //Knuth shuffle
        {
            int j = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        list.Sort((a, b) =>
        {
            float ra = Random.Range(0f, a.piece.weight);
            float rb = Random.Range(0f, b.piece.weight);
            return rb.CompareTo(ra);
        });
    }

    /// <summary>
    /// Does the 0==0, 1==1, 2==2 stuff. 
    /// </summary>
    bool EdgeMatch(EdgeType? required, TileType? requiredCategory, EdgeType candidate, TileType candidateCategory)
    {
        if (!required.HasValue) return true;

        return required.Value == candidate;
    }

    /// <summary>
    /// Goes through the grid and instantiates the prefab as a child of GameObject. Names the child descriptively based on tilePiece pieceName. 
    /// </summary>
    void InstantiateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var cell = grid[x, y];
                if (cell == null || cell.tilePiece.prefab == null) continue;
                Vector3 pos = new Vector3(x * tileSize, 0f, y * tileSize);
                Quaternion rot = Quaternion.Euler(0f, 90f * cell.rotationSteps, 0f);
                GameObject go = Instantiate(cell.tilePiece.GetRandomPrefab(), transform);
                go.transform.localPosition = pos;
                go.transform.localRotation = rot;
                go.name = $"{cell.tilePiece.pieceName}_{x}_{y}";
            }
        }
    }


    /// <summary>
    /// Checks if given coordinates are in the grid + if the cell is a road.
    /// </summary>
    bool IsRoad(int x, int y)
    {
        if (x < 0 || x >= gridWidth) return false;
        if (y < 0 || y >= gridHeight) return false;
        return roadMask[x, y];
    }

    /// <summary>
    /// Builds list of positions where a street should exist. It jumps forward a ranom distance between minBlock and maxBlock. 
    /// Image a grid with size = 10, 10x10 and minBlock=2, maxBlock=9
    /// R = [0, 4, 7]
    ///     0 1 2 3 4 5 6 7 8 9
    ///     R       R     R
    /// Because of this, the city will always be surrounded by roads cause 0 and size-1 will always be in the list.
    /// </summary>
    List<int> GenerateRoadLines(int size, int minBlock, int maxBlock)
    {
        List<int> roads = new();
        int pos = 0;
        roads.Add(pos);
        while (pos < size - 1)
        {
            pos += Random.Range(minBlock, maxBlock + 1);
            if (pos < size)
            {
                roads.Add(pos);
            }
        }
        //roads.Add(size-1);
        return roads;
    }

    /// <summary>
    /// Chooses a road for a road cell. Goes through all of the 4 possible road combinations at any rotation to see if it matches the edges of a road cell. 
    /// </summary>
    (TilePiece piece, int rotation) GetRoadTile(int x, int y)
    {
        bool north = IsRoad(x, y + 1);
        bool east = IsRoad(x + 1, y);
        bool south = IsRoad(x, y - 1);
        bool west = IsRoad(x - 1, y);

        foreach (var piece in tilePieces)
        {
            if (piece.category != TileType.Road)
            {
                continue;
            }
            for (int r = 0; r < 4; r++)
            {
                bool n = piece.GetEdge(Dir.North, r) == EdgeType.Road;
                bool e = piece.GetEdge(Dir.East, r) == EdgeType.Road;
                bool s = piece.GetEdge(Dir.South, r) == EdgeType.Road;
                bool w = piece.GetEdge(Dir.West, r) == EdgeType.Road;
                if (n == north && e == east && s == south && w == west)
                {
                    return (piece, r);
                }
            }
        }
        return (null, 0);
    }

}