using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManagerDefense))]
public class StateManagerDefense : StateManagerBuilding
{

    protected new EventManagerDefense eventManagerBuilding;

    public new EventManagerDefense EventManagerBuilding { get => eventManagerBuilding; }

    protected override void SetupStates()
    {
        base.SetupStates();
        listOfStates.Add(Constants.STATE_SIMULATION, new StateSimulationBuilding(this));
    }
}
