using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManagerDefense))]
public class StateManagerDefense : StateManagerBuilding
{
    public new EventManagerDefense EventManagerBuilding { get => (eventManagerBuilding as EventManagerDefense); }

    protected override void SetupStates()
    {
        base.SetupStates();
        listOfStates.Add(Constants.STATE_SIMULATION, new StateSimulationBuilding(this));
    }
}
