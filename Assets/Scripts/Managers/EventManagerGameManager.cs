using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerGameManager : EventManager
{

    public delegate void OnSetupInitialData(DataGame data);
    public OnSetupInitialData onSetupInitialData;

    public delegate void OnSpeedUpToggle(int value);
    public OnSpeedUpToggle onSpeedUpToggle;

    public delegate void OnBuildingButtonClick(Building building);
    public OnBuildingButtonClick onBuildingButtonClick;

    public delegate void OnBuildingDeselectButtonClick();
    public OnBuildingDeselectButtonClick onBuildingDeselectButtonClick;

    public delegate void OnBuildingClick(Building building);
    public OnBuildingClick onBuildingClick;

    public delegate void OnBuildingPlaced(Building building, List<Tile> tiles);
    public OnBuildingPlaced onBuildingPlaced;
    
    public delegate void OnBuildingOnTile(Building building);
    public OnBuildingOnTile onBuildingOnTile;

    public delegate void OnPlacingModeStarted();
    public OnPlacingModeStarted onPlacingModeStarted;

    public delegate void OnPlacingModeEnded();
    public OnPlacingModeEnded onPlacingModeEnded;

    public delegate void OnBuildingModeStarted();
    public OnBuildingModeStarted onBuildingModeStarted;

    public delegate void OnBuildingModeEnded();
    public OnBuildingModeEnded onBuildingModeEnded;

    public delegate void OnSimulationModeStarted();
    public OnSimulationModeStarted onSimulationModeStarted;

    public delegate void OnSimulationModeEnded();
    public OnSimulationModeEnded onSimulationModeEnded;

    public delegate void OnDraggingBuilding();
    public OnDraggingBuilding onDraggingBuilding;

    public delegate void OnStopDraggingBuilding();
    public OnStopDraggingBuilding onStopDraggingBuilding;

    public delegate void OnSpawnEnemy(Enemy enemy);
    public OnSpawnEnemy onSpawnEnemy;

    public delegate void OnEndMatch(bool result);
    public OnEndMatch onEndMatch;

    public delegate void OnDestroyableDestroyed();
    public OnDestroyableDestroyed onDestroyableDestroyed;
}
