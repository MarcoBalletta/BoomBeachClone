using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlacingBuilding : State
{
    public StatePlacingBuilding(StateManagerBuilding sm) : base(sm)
    {
        nameOfState = Constants.STATE_PLACING;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if ((stateManager as StateManagerBuilding).EventManagerBuilding?.onBuildingModeActivated != null)
            (stateManager as StateManagerBuilding).EventManagerBuilding?.onBuildingModeActivated();
        else
            Debug.Log("Empty");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        (stateManager as StateManagerBuilding).EventManagerBuilding?.onBuildingModeUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
