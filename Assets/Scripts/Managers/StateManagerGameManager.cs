using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagerGameManager : StateManager
{
    protected override void SetupStates()
    {
        listOfStates.Add(Constants.STATE_PLACING, new StatePlacing(this));
        listOfStates.Add(Constants.STATE_BUILDING_MODE, new StateBuildingMode(this));
        listOfStates.Add(Constants.STATE_SIMULATION, new StateSimulation(this));
    }

    protected override void Awake()
    {
        base.Awake();
        ChangeState(Constants.STATE_PLACING);
    }

    protected override void Update()
    {
        base.Update();
    }
}
