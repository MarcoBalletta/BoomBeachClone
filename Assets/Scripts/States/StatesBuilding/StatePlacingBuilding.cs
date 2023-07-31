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
