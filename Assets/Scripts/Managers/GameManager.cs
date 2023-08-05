using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EventManagerGameManager))]
[RequireComponent(typeof(StateManagerGameManager))]
public class GameManager : Singleton<GameManager>
{
    private EventManagerGameManager eventManager;
    private StateManagerGameManager stateManager;
    private GridManager gridManager;
    private NavMeshSurface navMesh;
    private Spawner spawner;
    private int simulationSpeed = 1;
    [Range(1,10)]
    [SerializeField] private int maxPlaceableBuildings = 1;
    [SerializeField] private Defense headquarterPrefab;
    [SerializeField] private Defense headquarterInstance;
    private int actualPlacedBuildings;
    private List<Defense> defenses = new List<Defense>();
    private List<Enemy> enemies = new List<Enemy>();

    public EventManagerGameManager EventManager { get => eventManager; }
    public Spawner Spawner { get => spawner; }
    public List<Defense> Defenses { get => defenses; }
    public int SimulationSpeed { get => simulationSpeed; }
    public StateManagerGameManager StateManager { get => stateManager; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerGameManager>();
        stateManager = GetComponent<StateManagerGameManager>();
        gridManager = GetComponentInChildren<GridManager>();
        navMesh = GetComponentInChildren<NavMeshSurface>();
        spawner = GetComponentInChildren<Spawner>();
        eventManager.onBuildingDeselectButtonClick += StartPlacingMode;
        eventManager.onBuildingButtonClick += StartBuildingPlacingMode;
        eventManager.onBuildingPlaced += PlaceBuilding;
        eventManager.onSimulationModeStarted += BakeNavMesh;
        eventManager.onSpawnEnemy += SpawnedEnemy;
    }

    private void Start()
    {
        RandomPlaceHeadquarter();
    }

    private void PlaceBuilding(Building building, Tile tile)
    { 
        building.transform.position = tile.GetPlacingPosition();
        tile.PlacedBuilding(building);
        if(building is Defense) 
        { 
            defenses.Add((building as Defense));
            (building as Defense).EventManager.onDead += RemoveDefense;
        }
        actualPlacedBuildings++;
        StartPlacingMode();
    }

    private void StartBuildingPlacingMode(Building building)
    {
        stateManager.ChangeState(Constants.STATE_BUILDING_MODE);
    }

    public void PlayModeActivated()
    {
        stateManager.ChangeState(Constants.STATE_SIMULATION);
    }

    private void BakeNavMesh()
    {
        navMesh.BuildNavMesh();
    }

    private void RemoveDefense(Defense defense)
    {
        defenses.Remove(defense);
        defenses.TrimExcess();
        if (defense == headquarterInstance)
            eventManager.onEndMatch(false);
    }

    private void StartPlacingMode()
    {
        stateManager.ChangeState(Constants.STATE_PLACING);
    }

    public bool CanPlaceOtherBuildings()
    {
        return actualPlacedBuildings < maxPlaceableBuildings;
    }

    public Vector3 GetCenterGrid()
    {
        return gridManager.GetCenterOfGrid();
    }

    public async void RandomPlaceHeadquarter()
    {
        headquarterInstance = Instantiate(headquarterInstance, transform.position, transform.rotation);
        gridManager.GenerateRowAndColumnRandom(out Vector2Int position);
        await System.Threading.Tasks.Task.Delay(50);
        headquarterInstance.PlaceBuilding(headquarterInstance, gridManager.MapTiles[position].tile);
        //headquarterInstance.transform.position = gridManager.GetWorld3DPosition(position);
    }

    private void SpawnedEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemy.EventManager.onDead += DeadEnemy;
        }
    }

    private void DeadEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            if (enemies.Count == 0)
                eventManager.onEndMatch(true);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
