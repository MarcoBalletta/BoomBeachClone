using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    public StateAttack(StateManagerEnemy sm) : base(sm)
    {
        nameOfState = Constants.STATE_ATTACK;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        (stateManager as StateManagerEnemy).EventManager?.onAttackStarted();
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
