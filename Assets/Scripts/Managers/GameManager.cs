using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EventManagerGameManager))]
[RequireComponent(typeof(StateManagerGameManager))]
public class GameManager : Singleton<GameManager>
{
    private EventManagerGameManager eventManager;
    private StateManagerGameManager stateManager;
    private NavMeshSurface navMesh;
    private Spawner spawner;
    private int simulationSpeed = 1;
    private List<Building> buildings = new List<Building>();
    private List<Enemy> enemies;

    public EventManagerGameManager EventManager { get => eventManager; }
    public Spawner Spawner { get => spawner; }
    public List<Building> Buildings { get => buildings; }
    public int SimulationSpeed { get => simulationSpeed; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerGameManager>();
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
        if(building is Defense)
            buildings.Add(building);
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
