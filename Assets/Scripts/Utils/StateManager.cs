using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    protected State currentState;
    protected Dictionary<string, State> listOfStates = new Dictionary<string, State>();

    protected virtual void SetupStates()
    {
        
    }

    protected virtual void Awake()
    {
        SetupStates();
    }

    protected virtual void Update()
    {
        currentState.OnUpdate();
    }

    public void ChangeState(string key)
    {
        if (currentState != null && currentState.nameOfState == key) return;
        currentState?.OnExit();
        currentState = listOfStates[key];
        currentState?.OnEnter();
    }
}
