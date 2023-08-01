using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManager))]
[RequireComponent(typeof(StateManagerGameManager))]
public class GameManager : Singleton<GameManager>
{
    private EventManager eventManager;
    private StateManagerGameManager stateManager;
    private int simulationSpeed = 1;
    public EventManager EventManager { get => eventManager; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManager>();
        stateManager = GetComponent<StateManagerGameManager>();
        eventManager.onBuildingPlaced += PlaceBuilding;
    }

    private void PlaceBuilding(Building building, Tile tile)
    { 
        building.transform.position = tile.GetPlacingPosition();
        tile.PlacedBuilding(building);
    }

    public void PlayModeActivated()
    {
        stateManager.ChangeState(Constants.STATE_SIMULATION);
    }
}
