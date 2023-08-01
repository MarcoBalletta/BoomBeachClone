using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSimulation : State
{
    public StateSimulation(StateManagerGameManager sm) : base(sm)
    {
        nameOfState = Constants.STATE_SIMULATION;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.instance.EventManager.onSimulationModeStarted();
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
