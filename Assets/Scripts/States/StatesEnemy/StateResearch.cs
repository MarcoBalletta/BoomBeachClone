using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateResearch : State
{
    public StateResearch(StateManagerEnemy sm) : base(sm)
    {
        nameOfState = Constants.STATE_RESEARCH;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        (stateManager as StateManagerEnemy).EventManager?.onResearchStarted();
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
