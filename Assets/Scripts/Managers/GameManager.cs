using System;
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
    [SerializeField] private float placementAngleRotation = 1;
    [SerializeField] private float placementSpeedRotation = 0.1f;
    [SerializeField] private Defense headquarterInstance;
    private int actualPlacedBuildings;
    private List<Defense> defenses = new List<Defense>();
    private List<Enemy> enemies = new List<Enemy>();
    
    public EventManagerGameManager EventManager { get => eventManager; }
    public Spawner Spawner { get => spawner; }
    public List<Defense> Defenses { get => defenses; }
    public int SimulationSpeed { get => simulationSpeed; }
    public StateManagerGameManager StateManager { get => stateManager; }
    public float PlacementAngleRotation { get => placementAngleRotation; }
    public float PlacementSpeedRotation { get => placementSpeedRotation; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerGameManager>();
        stateManager = GetComponent<StateManagerGameManager>();
        gridManager = GetComponentInChildren<GridManager>();
        navMesh = GetComponentInChildren<NavMeshSurface>();
        spawner = GetComponentInChildren<Spawner>();
    }

    private void OnEnable()
    {
        eventManager.onSetupInitialData += SetupGameManagerData;
        eventManager.onBuildingDeselectButtonClick += StartPlacingMode;
        eventManager.onBuildingClick += StartBuildingPlacingMode;
        eventManager.onBuildingPlaced += PlaceBuilding;
        eventManager.onSimulationModeStarted += BakeNavMesh;
        eventManager.onSpawnEnemy += SpawnedEnemy;
        eventManager.onDestroyableDestroyed += BakeNavMesh;
    }

    private void OnDisable()
    {
        eventManager.onSetupInitialData -= SetupGameManagerData;
        eventManager.onBuildingDeselectButtonClick -= StartPlacingMode;
        eventManager.onBuildingClick -= StartBuildingPlacingMode;
        eventManager.onBuildingPlaced -= PlaceBuilding;
        eventManager.onSimulationModeStarted -= BakeNavMesh;
        eventManager.onSpawnEnemy -= SpawnedEnemy;
        eventManager.onDestroyableDestroyed -= BakeNavMesh;
    }

    private void Start()
    {
        if(DataForGameHandler.instance != null)
            eventManager.onSetupInitialData(DataForGameHandler.instance.DataGame);
        eventManager.onSpeedUpToggle(simulationSpeed);
        RandomPlaceHeadquarter();
    }

    private void SetupGameManagerData(DataGame data)
    {
        maxPlaceableBuildings = data.numberOfPlaceableBuildings;
    }

    //toggles speed up button in simulation mode
    public void ToggleSpeedUpButton()
    {
        if (simulationSpeed == 1)
            simulationSpeed = 2;
        else
            simulationSpeed = 1;
        eventManager.onSpeedUpToggle(simulationSpeed);
    }

    //places the building in center of the tiles under
    private void PlaceBuilding(Building building, List<Tile> tiles)
    { 
        building.transform.position = GetCenterOfTiles(tiles);
        foreach (var tile in tiles) 
        { 
            tile.PlacedBuilding(building);
        }
        if(building is Defense && !defenses.Contains(building as Defense)) 
        { 
            defenses.Add((building as Defense));
            building.EventManager.onDead += RemoveDefense;
        }
        actualPlacedBuildings++;
        StartPlacingMode();
    }

    //gets center position of the list of tiles in order to place the building
    private Vector3 GetCenterOfTiles(List<Tile> tiles)
    {
        Vector3 totalPositions = Vector3.zero;
        foreach(var tile in tiles)
        {
            totalPositions += tile.GetPlacingPosition();
        }
        return totalPositions / tiles.Count;
    }

    private void StartBuildingPlacingMode(Building building)
    {
        stateManager.ChangeState(Constants.STATE_BUILDING_MODE);
    }

    public bool IsGameStarted()
    {
        return stateManager.CurrentState.nameOfState == Constants.STATE_SIMULATION;
    }

    public void PlayModeActivated()
    {
        stateManager.ChangeState(Constants.STATE_SIMULATION);
    }

    private void BakeNavMesh()
    {
        navMesh.BuildNavMesh();
    }

    public void RemoveDefense(Building defense)
    {
        if(defenses.Contains(defense as Defense))
        {
            defenses.Remove((defense as Defense));
            defenses.TrimExcess();
            if (stateManager.CurrentState.nameOfState == Constants.STATE_PLACING)
                actualPlacedBuildings--;
            else if(IsGameStarted())
                if (defense != null && defense == headquarterInstance)
                    eventManager.onEndMatch(false);
        }
    }

    private void StartPlacingMode()
    {
        stateManager.ChangeState(Constants.STATE_PLACING);
    }

    public bool CanPlaceOtherBuildings()
    {
        return actualPlacedBuildings < maxPlaceableBuildings + 1;
    }

    public Vector3 GetCenterGrid()
    {
        return gridManager.GetCenterOfGrid();
    }

    //random place headquarter in grid
    public async void RandomPlaceHeadquarter()
    {
        headquarterInstance = Instantiate(headquarterInstance, transform.position + transform.up * 5, transform.rotation);
        await System.Threading.Tasks.Task.Delay(100);
        while (!headquarterInstance.CheckIfTilesAreUnderBuilding())
        {
            gridManager.GenerateRowAndColumnRandom(out Vector2Int position);
            headquarterInstance.transform.position = gridManager.GetWorld3DPosition(position) + Vector3.up*2;
            headquarterInstance.PlacedState();
            headquarterInstance.CheckTilesUnderBuilding();
        }
        headquarterInstance.PlaceBuilding(headquarterInstance, headquarterInstance.GetTilesUnder());
    }

    //adds enemy in list
    private void SpawnedEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemy.EventManager.onDead += DeadEnemy;
            enemies.Add(enemy);
        }
    }

    //removes enemy from list
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

    public void BackToMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE_NAME);
    }
}   
