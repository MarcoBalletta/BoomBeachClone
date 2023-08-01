using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(StateManagerBuilding))]
[RequireComponent(typeof(EventManagerBuilding))]
public class Building : MonoBehaviour
{
    [SerializeField] private DefenseData data;
    private StateManagerBuilding stateManager;
    private EventManagerBuilding eventManager;
    [SerializeField] private LayerMask layerMask;
    private SphereCollider coll;
    private Tile tileUnder;

    public EventManagerBuilding EventManager { get => eventManager; set => eventManager = value; }

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        stateManager = GetComponent<StateManagerBuilding>();
        eventManager = GetComponent<EventManagerBuilding>();
        eventManager.onBuildingModeActivated += StartBuildingMode;
        eventManager.onBuildingModeUpdate += CheckTileUnderBuilding;
        eventManager.onBuildingModeReleased += PlaceBuildingIfPossible;
    }

    public void Setup()
    {
        coll.radius = data.range;
    }

    private void StartBuildingMode()
    {
        Debug.Log("StartBuildingMode");
    }
    
    private void CheckTileUnderBuilding()
    {
        if(Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit , 3f, layerMask, QueryTriggerInteraction.Collide))
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

    private void DeselectTile()
    {
        if (tileUnder != null)
            tileUnder.DeselectedTile();
    }

    private bool CheckIfTileIsUnder()
    {
        return tileUnder != null;
    }

    private void PlaceBuildingIfPossible()
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
