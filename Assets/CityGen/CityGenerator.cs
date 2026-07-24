using UnityEngine;
using System.Collections.Generic;

public class CityGenerator : MonoBehaviour
{
    public List<TilePiece> tilePieces;
    public int gridWidth = 10;
    public int gridHeight = 10;

    public float tileSize = 16;

    public int maxAttempts = 1000;

    public bool requireBorder = true;

    //public BuildingKit[] buildingKits;

    [Tooltip("Maximum width of a lot in cells. If a building's width exceeds this, it will be split into multiple lots.")]
    public int maxLotWidthCells = 3;

    private class PlacedTile
    {
        public TilePiece tilePiece;
        public int rotationSteps;
    }

    private PlacedTile[,] grid;
    private int attemptsUsed;

    void Start()
    {
        GenerateCity();
    }

    public void GenerateCity()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        grid = new PlacedTile[gridWidth, gridHeight];
        attemptsUsed = 0;
        bool success = TryFillGrid(0, 0);
        if (!success)
        {
            Debug.LogWarning("CityGenerator: could not find a fully valid layout");
        }

        InstantiateGrid();
    }

    bool TryFillGrid(int x, int y)
    {
        if (y >= gridHeight)
        {
            return true; // Successfully filled the grid
        }

        int nextX = (x + 1);
        int nextY = y;
        if (nextX >= gridWidth)
        {
            nextX = 0;
            nextY = y + 1;
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

    List<(TilePiece piece, int rotationSteps)> GetValidTiles(int x, int y)
    {
        var result = new List<(TilePiece piece, int rotationSteps)>();
        EdgeType? westNeighborEastSocket = null;
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
            if (piece.prefab == null) continue;

            int rotStep = 4 / Mathf.Max(1, 4);
            for (int r = 0; r < 4; r += rotStep)
            {
                if (!EdgeMatch(westNeighborEastSocket, westNeighborCategory, piece.GetEdge(Dir.West, r), piece.category)) continue;
                if (!EdgeMatch(southNeighborNorthSocket, southNeighborCategory, piece.GetEdge(Dir.South, r), piece.category)) continue;
                if (requireBorder)
                {
                    if (x == 0 && piece.GetEdge(Dir.West, r) != EdgeType.Empty) continue;
                    if (x == gridWidth - 1 && piece.GetEdge(Dir.East, r) != EdgeType.Empty) continue;
                    if (y == 0 && piece.GetEdge(Dir.South, r) != EdgeType.Empty) continue;
                    if (y == gridHeight - 1 && piece.GetEdge(Dir.North, r) != EdgeType.Empty) continue;
                }

                result.Add((piece, r));
            }
        }
        return result;
    }
    void WeightedShuffle(List<(TilePiece piece, int rotationSteps)> list)
    {
        for (int i = 0; i < list.Count; i++)
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

    bool EdgeMatch(EdgeType? required, TileType? requiredCategory, EdgeType candidate, TileType candidateCategory)
    {
        if (!required.HasValue) return true;
        if (required.Value != candidate) return false;
        if (required.Value == EdgeType.Sidewalk &&
            requiredCategory.HasValue && requiredCategory.Value == TileType.Building &&
            candidateCategory == TileType.Building)
        {
            return false;
        }
 
        return true;
    }

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

                GameObject go = Instantiate(cell.tilePiece.prefab, transform);
                go.transform.localPosition = pos;
                go.transform.localRotation = rot;
                go.name = $"{cell.tilePiece.pieceName}_{x}_{y}";
            }
        }
    }
}