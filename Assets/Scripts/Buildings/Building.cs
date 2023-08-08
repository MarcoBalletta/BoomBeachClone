using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Controller
{
    protected BoxCollider coll;
    //private Tile tileUnder;
    [SerializeField] protected LayerMask layerMask;
    protected StateManagerBuilding stateManager;
    protected EventManagerBuilding eventManager;
    [SerializeField] protected GameObject panelUIBuilding;
    [SerializeField] private List<RaycastUnderTileData> checkTilesUnderPoints = new List<RaycastUnderTileData>();

    public EventManagerBuilding EventManager { get => eventManager; set => eventManager = value; }
    //public Tile TileUnder { get => tileUnder; }

    protected virtual void Awake()
    {
        coll = GetComponent<BoxCollider>();
        stateManager = GetComponent<StateManagerBuilding>();
        eventManager = GetComponent<EventManagerBuilding>();
    }

    protected virtual void OnEnable()
    {
        eventManager.onBuildingModeUpdate += CheckTilesUnderBuilding;
        eventManager.onBuildingModeReleased += PlaceBuildingIfPossible;
    }

    protected virtual void OnDisable()
    {
        eventManager.onBuildingModeUpdate -= CheckTilesUnderBuilding;
        eventManager.onBuildingModeReleased -= PlaceBuildingIfPossible;
    }

    public void StartBuildingMode()
    {
        //show UI 
        stateManager.ChangeState(Constants.STATE_PLACING);
    }

    //protected void CheckTileUnderBuilding()
    //{
    //    if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit, 3f, layerMask, QueryTriggerInteraction.Collide))
    //    {
    //        if (hit.collider.TryGetComponent(out Tile tileHit) && !hit.collider.GetComponent<Tile>().IsOccupied())
    //        {
    //            DeselectTile();
    //            tileUnder = tileHit;
    //            tileUnder.SelectedTile();
    //        }
    //    }
    //    else
    //    {
    //        DeselectTile();
    //        tileUnder = null;
    //    }
    //}

    public void CheckTilesUnderBuilding()
    {
        foreach (var raycastPosition in checkTilesUnderPoints.ToList()) 
        {
            Debug.DrawRay(transform.position + transform.right * raycastPosition.position.x + transform.forward * raycastPosition.position.z, transform.up * -1, Color.black, 5f);
            if (Physics.Raycast(transform.position + transform.right * raycastPosition.position.x + transform.forward * raycastPosition.position.z, transform.up * -1, out RaycastHit hit, 3f, layerMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.TryGetComponent(out Tile tileHit) && !hit.collider.GetComponent<Tile>().IsOccupied())
                {
                    if (!CheckIfTileCanBeUnderTile(tileHit)) continue;
                    DeselectTile(raycastPosition.underTile);
                    int index = checkTilesUnderPoints.IndexOf(raycastPosition);
                    var structElementToModify = checkTilesUnderPoints[index];
                    structElementToModify.underTile = tileHit;
                    checkTilesUnderPoints[index] = structElementToModify;
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

    private void DeselectUnderTile(RaycastUnderTileData data)
    {
        DeselectTile(data.underTile);
        int index = checkTilesUnderPoints.IndexOf(data);
        var structElementToModify = checkTilesUnderPoints[index];
        structElementToModify.underTile = null;
        checkTilesUnderPoints[index] = structElementToModify;
    }

    private bool CheckIfTileCanBeUnderTile(Tile tile)
    {
        foreach(var underTile in checkTilesUnderPoints)
        {
            if (underTile.underTile == tile) return false;
        }
        return true;
    }

    //protected void DeselectTile()
    //{
    //    if (tileUnder != null)
    //        tileUnder.DeselectedTile();
    //}

    protected void DeselectTile(Tile tile)
    {
        if (tile != null)
            tile.DeselectedTile();
    }

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

    //protected bool CheckIfTileIsUnder()
    //{
    //    return tileUnder != null;
    //}

    protected bool CheckIfTilesAreUnderBuilding()
    {
        foreach(var tilesUnder in checkTilesUnderPoints)
        {
            if (tilesUnder.underTile == null)
                return false;
        }
        return true;
    }

    public void PlaceBuildingIfPossible()
    {
        if (CheckIfTilesAreUnderBuilding())
        {
            //can place building
            PlaceBuilding(this, GetTilesUnder());
        }
    }

    public List<Tile> GetTilesUnder()
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var underTile in checkTilesUnderPoints)
        {
            tiles.Add(underTile.underTile);
        }
        return tiles;
    }

    public void PlaceBuilding(Building building, List<Tile> tiles)
    {
        GameManager.instance.EventManager.onBuildingPlaced(building, tiles);
        PlacedState();
    }

    public void PlacedState()
    {
        stateManager.ChangeState(Constants.STATE_PLACED);
    }

    public void DeselectedBuilding()
    {
        foreach(var underTile in checkTilesUnderPoints)
        {
            if (underTile.underTile != null)
                underTile.underTile.DeselectedTile();
        }
        //if (tileUnder)
        //    tileUnder.DeselectedTile();
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
