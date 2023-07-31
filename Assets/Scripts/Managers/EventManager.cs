using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnBuildingSelected(Building building);
    public OnBuildingSelected onBuildingSelected;
    public delegate void OnBuildingReleased(Building building);
    public OnBuildingReleased onBuildingReleased;
    public delegate void OnBuildingOnTile(Building building);
    public OnBuildingOnTile onBuildingOnTile;
}
