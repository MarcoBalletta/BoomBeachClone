using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerGameManager : EventManager
{
    public delegate void OnBuildingButtonClick(Building building);
    public OnBuildingButtonClick onBuildingButtonClick;

    public delegate void OnBuildingDeselectButtonClick();
    public OnBuildingDeselectButtonClick onBuildingDeselectButtonClick;

    public delegate void OnBuildingClick(Building building);
    public OnBuildingClick onBuildingClick;

    public delegate void OnBuildingPlaced(Building building, Tile tile);
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
}
