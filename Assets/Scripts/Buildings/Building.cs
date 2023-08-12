using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Controller
{
    protected BoxCollider coll;
    [SerializeField] protected LayerMask layerMask;
    protected StateManagerBuilding stateManager;
    protected EventManagerBuilding eventManager;
    [SerializeField] protected GameObject panelUIBuilding;
    [SerializeField] private List<RaycastUnderTileData> checkTilesUnderPoints = new List<RaycastUnderTileData>();

    public EventManagerBuilding EventManager { get => eventManager; set => eventManager = value; }

    protected virtual void Awake()
    {
        coll = GetComponent<BoxCollider>();
        stateManager = GetComponent<StateManagerBuilding>();
        eventManager = GetComponent<EventManagerBuilding>();
    }

    protected virtual void OnEnable()
    {
        eventManager.onBuildingModeActivated += DeselectUnderTiles;
        eventManager.onBuildingModeUpdate += CheckTilesUnderBuilding;
        eventManager.onBuildingModeReleased += PlaceBuildingIfPossible;
    }

    protected virtual void OnDisable()
    {
        eventManager.onBuildingModeActivated -= DeselectUnderTiles;
        eventManager.onBuildingModeUpdate -= CheckTilesUnderBuilding;
        eventManager.onBuildingModeReleased -= PlaceBuildingIfPossible;
    }

    public void StartBuildingMode()
    {
        stateManager.ChangeState(Constants.STATE_PLACING);
    }

    //raycast down to see if the raycasts positions are hitting a tile
    public void CheckTilesUnderBuilding()
    {
        foreach (var raycastPosition in checkTilesUnderPoints.ToList()) 
        {
            if (Physics.Raycast(transform.position + transform.right * raycastPosition.position.x + transform.forward * raycastPosition.position.z, transform.up * -1, out RaycastHit hit, 3f, layerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.TryGetComponent(out Tile tileHit) && !hit.collider.GetComponent<Tile>().IsOccupied())
                {
                    if (!CheckIfTileCanBeUnderTile(tileHit)) continue;
                    DeselectTile(raycastPosition.underTile);
                    FindRaycastAndModifyTileUnder(raycastPosition, tileHit);
                    tileHit.SelectedTile();
                }
                else
                {
                    DeselectUnderTile(raycastPosition);
                }
            }
            else
            {
                DeselectUnderTile(raycastPosition);
            }
        }
    }

    private void DeselectUnderTiles()
    {
        foreach(var raycast in checkTilesUnderPoints.ToList())
        {
            DeselectUnderTile(raycast);
        }
    }

    //deselect the under tile that was selected, changing data and material
    private void DeselectUnderTile(RaycastUnderTileData data)
    {
        DeselectTile(data.underTile);
        FindRaycastAndModifyTileUnder(data, null);
    }

    //finds the raycast position and modifies the tile under with given tile
    private void FindRaycastAndModifyTileUnder(RaycastUnderTileData data, Tile value)
    {
        int index = checkTilesUnderPoints.IndexOf(data);
        var structElementToModify = checkTilesUnderPoints[index];
        structElementToModify.underTile = value;
        checkTilesUnderPoints[index] = structElementToModify;
    }

    //checks if the tile isn't already an under tile for another raycast position
    private bool CheckIfTileCanBeUnderTile(Tile tile)
    {
        foreach(var underTile in checkTilesUnderPoints)
        {
            if (underTile.underTile == tile) return false;
        }
        return true;
    }

    //deselects the tile
    protected void DeselectTile(Tile tile)
    {
        if (tile != null)
            tile.DeselectedTile();
    }

    //when the player clicks on the rotate button, rotates the building with an angle set in the game manager
    public IEnumerator RotateLerpBuilding()
    {
        Vector3 eulerOriginal = transform.rotation.eulerAngles;
        Vector3 eulerObj = new Vector3(eulerOriginal.x, eulerOriginal.y + GameManager.instance.PlacementAngleRotation, eulerOriginal.z);
        Quaternion rotObjective = Quaternion.Euler(eulerObj);
        while (Mathf.Abs(Quaternion.Dot(transform.rotation, rotObjective)) < 0.99) 
        {
            transform.rotation = Quaternion.Lerp(transform.localRotation, rotObjective, GameManager.instance.PlacementSpeedRotation);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.rotation = rotObjective;
    }

    //checks if every raycast has a tile under
    public bool CheckIfTilesAreUnderBuilding()
    {
        foreach(var tilesUnder in checkTilesUnderPoints)
        {
            if (tilesUnder.underTile == null)
                return false;
        }
        return true;
    }

    //if every raycast has a tile under place the building
    public void PlaceBuildingIfPossible()
    {
        if (CheckIfTilesAreUnderBuilding())
        {
            //can place building
            PlaceBuilding(this, GetTilesUnder());
        }
    }

    //gets all the tiles under
    public List<Tile> GetTilesUnder()
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var underTile in checkTilesUnderPoints)
        {
            tiles.Add(underTile.underTile);
        }
        return tiles;
    }

    //places the building, calls event to game manager, changes the state in placed
    public void PlaceBuilding(Building building, List<Tile> tiles)
    {
        GameManager.instance.EventManager.onBuildingPlaced(building, tiles);
        PlacedState();
    }

    public void PlacedState()
    {
        stateManager.ChangeState(Constants.STATE_PLACED);
    }

    //clicked on the deselect button in the placing UI, deselects the building and destroys it
    public void DeselectedBuilding()
    {
        foreach(var underTile in checkTilesUnderPoints)
        {
            if (underTile.underTile != null)
                underTile.underTile.DeselectedTile();
        }
        if(this is Defense)
        {
            GameManager.instance.RemoveDefense(this as Defense);
        }
        Destroy(gameObject);
    }

    protected virtual void SetupUIAfterPlacingBuilding()
    {
        panelUIBuilding.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.EventManager.onDestroyableDestroyed();
    }
}