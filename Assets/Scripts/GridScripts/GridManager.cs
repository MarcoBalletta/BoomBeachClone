using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int maxRow;
    [SerializeField] private int maxColumn;
    [SerializeField] private Dictionary<Vector2Int, TileData> mapTiles = new Dictionary<Vector2Int, TileData>();
    private Grid gridData;
    public delegate void GridGenerated(Dictionary<Vector2Int, TileData> mapTiles);
    public GridGenerated onGridGenerated;
    [SerializeField] private Vector3 offsetGrid;

    public int MaxRow { get => maxRow; }
    public int MaxColumn { get => maxColumn; }
    public Dictionary<Vector2Int, TileData> MapTiles { get => mapTiles; }

    private void Awake()
    {
        gridData = GetComponent<Grid>();
        GenerateGrid();
    }

    #region GenerationGrid
    //generates the grid, spawns the tiles
    private void GenerateGrid()
    {
        for (int row = 0; row < maxRow; row++)
        {
            for (int column = 0; column < maxColumn; column++)
            {
                var tile = Instantiate(tilePrefab, GetWorld3DPosition(new Vector2Int(row, column)), Quaternion.identity, transform);
                tile.transform.localScale = gridData.cellSize;
                tile.Initialize(this, row, column, tile);
                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                mapTiles[new Vector2Int(row, column)] = tile.data;
            }
        }
        if (onGridGenerated != null) onGridGenerated(mapTiles);
    }

    #region GetWorld2DPosition
    public Vector2 GetWorld2DPosition(Vector2Int position)
    {
        return new Vector2(position.x * (gridData.cellSize.x + gridData.cellGap.x), position.y * (gridData.cellSize.z + gridData.cellGap.z));
    }

    public Vector2 GetWorld2DPosition(int x, int y)
    {
        return new Vector2(x * (gridData.cellSize.x + gridData.cellGap.x), y * (gridData.cellSize.z + gridData.cellGap.z));
    }
    #endregion

    #region GetWorld3DPosition
    public Vector3 GetWorld3DPosition(Vector2Int position)
    {
        return new Vector3(position.x * (gridData.cellSize.x + gridData.cellGap.x) + offsetGrid.x, 0 + offsetGrid.y, position.y * (gridData.cellSize.z + gridData.cellGap.z ) + offsetGrid.z);
    }

    public Vector3 GetWorld3DPosition(int x, int y)
    {
        return new Vector3(x * (gridData.cellSize.x + gridData.cellGap.x), 0, y * (gridData.cellSize.z + gridData.cellGap.z));
    }
    #endregion

    //checks if the coordinates are in the grid bounds
    public bool CheckGridBounds(Vector2Int coordinates)
    {
        return (coordinates.x < 0 || coordinates.x >= maxRow || coordinates.y < 0 || coordinates.y >= maxColumn);
    }

    //public bool CheckWalkable(Vector2Int coordinates)
    //{
    //    return mapTiles[coordinates].walkable;
    //}
    #endregion

    #region GenerateRandomRowAndColumn
    public void GenerateRowAndColumnRandom(out Vector2Int position)
    {
        int randomRow = Random.Range(0, maxRow);
        int randomColumn = Random.Range(0, maxColumn);
        position = new Vector2Int(randomRow, randomColumn);
    }

    public void GenerateRowAndColumnRandom(out int randomRow, out int randomColumn)
    {
        randomRow = Random.Range(0, maxRow);
        randomColumn = Random.Range(0, maxColumn);
    }
    #endregion

    public Vector3 GetCenterOfGrid()
    {
        Vector3 startGrid = GetWorld3DPosition(0, 0);
        Vector3 endGrid = GetWorld3DPosition(maxRow - 1, maxColumn - 1);

        return new Vector3((startGrid.x + endGrid.x) / 2, 0, (startGrid.z + endGrid.z) / 2);
    }

    //public float HeightCamera()
    //{
    //    return maxColumn * (gridData.cellGap.z + gridData.cellSize.z) + (gridData.cellGap.y + gridData.cellSize.y) + 1;
    //}

    //public bool CheckIfTileIsWalkable(Vector2Int positionGrid)
    //{
    //    return mapTiles[positionGrid].walkable;
    //}
}
