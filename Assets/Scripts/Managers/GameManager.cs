using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EventManager))]
[RequireComponent(typeof(StateManagerGameManager))]
public class GameManager : Singleton<GameManager>
{
    private EventManager eventManager;
    private StateManagerGameManager stateManager;
    private NavMeshSurface navMesh;
    private Spawner spawner;
    private int simulationSpeed = 1;

    public EventManager EventManager { get => eventManager; }
    public Spawner Spawner { get => spawner; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManager>();
        stateManager = GetComponent<StateManagerGameManager>();
        navMesh = GetComponentInChildren<NavMeshSurface>();
        spawner = GetComponentInChildren<Spawner>();
        eventManager.onBuildingPlaced += PlaceBuilding;
        eventManager.onSimulationModeStarted += BakeNavMesh;
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

    private void BakeNavMesh()
    {
        navMesh.BuildNavMesh();
    }
}
