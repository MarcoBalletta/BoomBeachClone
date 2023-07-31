using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSimulationBuilding : State
{
    public StateSimulationBuilding(StateManagerBuilding sm) : base(sm)
    {
        nameOfState = Constants.STATE_SIMULATION;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
