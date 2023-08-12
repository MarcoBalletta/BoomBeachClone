using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData data;
    [SerializeField] private Material normal;
    [SerializeField] private Material selected;
    private Renderer rend;
    private Building buildingOccupying;
    [SerializeField] private Vector3 placingPosition;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void Initialize(GridManager gridM, int rowInit, int columnInit, Tile tile)
    {
        data = new TileData(gridM, rowInit, columnInit, tile);
    }

    private void ChangeMaterial(Material material)
    {
        rend.material = material;
    }

    public void SelectedTile()
    {
        ChangeMaterial(selected);
    }

    public void DeselectedTile()
    {
        ChangeMaterial(normal);
        buildingOccupying = null;
    }

    public void PlacedBuilding(Building building)
    {
        DeselectedTile();
        buildingOccupying = building;
    }

    public Vector3 GetPlacingPosition()
    {
        return transform.position + placingPosition;
    }

    public bool IsOccupied()
    {
        return buildingOccupying != null;
    }
}
