using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : Controller
{
    protected BoxCollider coll;
    private Tile tileUnder;
    [SerializeField] protected LayerMask layerMask;
    protected StateManagerBuilding stateManager;
    protected EventManagerBuilding eventManager;
    [SerializeField] protected GameObject panelUIBuilding;
    
    public EventManagerBuilding EventManager { get => eventManager; set => eventManager = value; }
    public Tile TileUnder { get => tileUnder; }

    protected virtual void Awake()
    {
        coll = GetComponent<BoxCollider>();
        stateManager = GetComponent<StateManagerBuilding>();
        eventManager = GetComponent<EventManagerBuilding>();
    }

    protected virtual void OnEnable()
    {
        //eventManager.onBuildingModeActivated += StartBuildingMode;
        eventManager.onBuildingModeUpdate += CheckTileUnderBuilding;
        eventManager.onBuildingModeReleased += PlaceBuildingIfPossible;
    }

    protected virtual void OnDisable()
    {
        //eventManager.onBuildingModeActivated -= StartBuildingMode;
        eventManager.onBuildingModeUpdate -= CheckTileUnderBuilding;
        eventManager.onBuildingModeReleased -= PlaceBuildingIfPossible;
    }

    public void StartBuildingMode()
    {
        //show UI 
        stateManager.ChangeState(Constants.STATE_PLACING);
    }

    protected void CheckTileUnderBuilding()
    {
        if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit, 3f, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.GetComponent<Tile>() && !hit.collider.GetComponent<Tile>().IsOccupied())
            {
                DeselectTile();
                tileUnder = hit.collider.GetComponent<Tile>();
                tileUnder.SelectedTile();
            }
        }
        else
        {
            DeselectTile();
            tileUnder = null;
        }
    }

    protected void DeselectTile()
    {
        if (tileUnder != null)
            tileUnder.DeselectedTile();
    }

    public IEnumerator RotateLerpBuilding()
    {
        Vector3 eulerOriginal = transform.rotation.eulerAngles;
        Vector3 eulerObj = new Vector3(eulerOriginal.x, eulerOriginal.y + GameManager.instance.PlacementAngleRotation, eulerOriginal.z);
        Quaternion rotObjective = Quaternion.Euler(eulerObj);
        while (Mathf.Abs(Quaternion.Dot(transform.rotation, rotObjective)) < 0.99) 
        {
            Debug.Log("Rotating from : " + transform.rotation + " to : " + rotObjective + " ---- dot : "  + Quaternion.Dot(transform.rotation, rotObjective));
            Debug.Log("Rotating from : " + transform.rotation.eulerAngles + " to : " + rotObjective.eulerAngles + " ---- dot : "  + Quaternion.Dot(transform.rotation, rotObjective));
            transform.rotation = Quaternion.Lerp(transform.localRotation, rotObjective, GameManager.instance.PlacementSpeedRotation);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.rotation = rotObjective;
    }

    protected bool CheckIfTileIsUnder()
    {
        return tileUnder != null;
    }

    public void PlaceBuildingIfPossible()
    {
        if (CheckIfTileIsUnder())
        {
            //can place building
            PlaceBuilding(this, tileUnder);
        }
    }

    public void PlaceBuilding(Building building, Tile tile)
    {
        GameManager.instance.EventManager.onBuildingPlaced(building, tile);
        stateManager.ChangeState(Constants.STATE_PLACED);
    }

    public void DeselectedBuilding()
    {
        if (tileUnder)
            tileUnder.DeselectedTile();
        Destroy(gameObject);
    }

    private void RotateBuilding(float angle)
    {
        //lerp rotation building when click on rotation button
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
