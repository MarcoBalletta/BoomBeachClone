using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBuildingMode : State
{
    public StateBuildingMode(StateManagerGameManager sm) : base(sm)
    {
        nameOfState = Constants.STATE_BUILDING_MODE;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(GameManager.instance.EventManager.onBuildingModeStarted != null)
            GameManager.instance.EventManager.onBuildingModeStarted();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (GameManager.instance.EventManager.onBuildingModeEnded != null)
            GameManager.instance.EventManager.onBuildingModeEnded();
    }
}
