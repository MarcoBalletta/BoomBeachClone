using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlacedBuilding : State
{

    public StatePlacedBuilding(StateManagerBuilding sm) : base(sm)
    {
        nameOfState = Constants.STATE_PLACED;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        (stateManager as StateManagerBuilding).EventManagerBuilding?.onPlacedBuilding();
    }
}
