using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerBuilding : MonoBehaviour
{
    public delegate void BuildingModeActivated();
    public BuildingModeActivated onBuildingModeActivated;

    public delegate void BuildingModeUpdate();
    public BuildingModeActivated onBuildingModeUpdate;

    public delegate void BuildingModeReleased();
    public BuildingModeReleased onBuildingModeReleased;

    public delegate void PlacingModeActivated();
    public PlacingModeActivated onPlacingModeActivated;
}
