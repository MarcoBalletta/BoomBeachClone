using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Building : MonoBehaviour
{
    protected BoxCollider coll;
    protected Tile tileUnder;
    [SerializeField] protected LayerMask layerMask;
    protected StateManagerBuilding stateManager;
    protected EventManagerBuilding eventManager;
    public EventManagerBuilding EventManager { get => eventManager; set => eventManager = value; }

    protected virtual void Awake()
    {
        coll = GetComponent<BoxCollider>();
        stateManager = GetComponent<StateManagerBuilding>();
        eventManager = GetComponent<EventManagerBuilding>();
    }

    protected virtual void OnEnable()
    {
        eventManager.onBuildingModeActivated += StartBuildingMode;
        eventManager.onBuildingModeUpdate += CheckTileUnderBuilding;
        eventManager.onBuildingModeReleased += PlaceBuildingIfPossible;
    }

    protected virtual void OnDisable()
    {
        eventManager.onBuildingModeActivated -= StartBuildingMode;
        eventManager.onBuildingModeUpdate -= CheckTileUnderBuilding;
        eventManager.onBuildingModeReleased -= PlaceBuildingIfPossible;
    }

    protected void StartBuildingMode()
    {
        Debug.Log("StartBuildingMode");
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

    protected bool CheckIfTileIsUnder()
    {
        return tileUnder != null;
    }

    protected void PlaceBuildingIfPossible()
    {
        if (CheckIfTileIsUnder())
        {
            //can place building
            GameManager.instance.EventManager.onBuildingPlaced(this, tileUnder);
            stateManager.ChangeState(Constants.STATE_PLACED);
        }
        else
        {
            //can't place building
            Destroy(gameObject);
        }
    }
}
