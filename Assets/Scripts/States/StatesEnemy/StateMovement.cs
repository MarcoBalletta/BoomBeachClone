using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMovement : State
{
    public StateMovement(StateManagerEnemy sm) : base(sm)
    {
        nameOfState = Constants.STATE_MOVEMENT;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        (stateManager as StateManagerEnemy).EventManager?.onMovementStarted();
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
