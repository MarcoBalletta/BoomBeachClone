using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    protected StateManager stateManager;
    public string nameOfState;

    public State(StateManager sm)
    {
        stateManager = sm;
    }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}