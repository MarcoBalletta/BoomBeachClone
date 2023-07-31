using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagerBuilding : StateManager
{
    protected override void SetupStates()
    {
        listOfStates.Add(Constants.STATE_PLACING, new StatePlacingBuilding(this));
        listOfStates.Add(Constants.STATE_SIMULATION, new StateSimulationBuilding(this));
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        ChangeState(Constants.STATE_PLACING);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
