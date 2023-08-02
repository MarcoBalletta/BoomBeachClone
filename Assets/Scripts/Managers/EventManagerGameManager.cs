using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerGameManager : EventManager
{
    public delegate void OnBuildingSelected(Building building);
    public OnBuildingSelected onBuildingSelected;
    
    public delegate void OnBuildingPlaced(Building building, Tile tile);
    public OnBuildingPlaced onBuildingPlaced;
    
    public delegate void OnBuildingOnTile(Building building);
    public OnBuildingOnTile onBuildingOnTile;

    public delegate void OnBuildingModeEnded();
    public OnBuildingModeEnded onBuildingModeEnded;

    public delegate void OnSimulationModeStarted();
    public OnSimulationModeStarted onSimulationModeStarted;
}
