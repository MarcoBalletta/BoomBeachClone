using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlacing : State
{
    public StatePlacing(StateManagerGameManager sm) : base(sm)
    {
        nameOfState = Constants.STATE_PLACING;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(GameManager.instance.EventManager?.onPlacingModeStarted != null)
            GameManager.instance.EventManager?.onPlacingModeStarted();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (GameManager.instance.EventManager?.onPlacingModeEnded != null)
            GameManager.instance.EventManager.onPlacingModeEnded();
    }
}
