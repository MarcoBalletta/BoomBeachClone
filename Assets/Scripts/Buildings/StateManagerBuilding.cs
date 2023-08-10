using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManagerBuilding))]
public class StateManagerBuilding : StateManager
{

    protected EventManagerBuilding eventManagerBuilding;

    public virtual EventManagerBuilding EventManagerBuilding { get => eventManagerBuilding; }

    protected override void SetupStates()
    {
        listOfStates.Add(Constants.STATE_PLACING, new StatePlacingBuilding(this));
        listOfStates.Add(Constants.STATE_PLACED, new StatePlacedBuilding(this));
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        eventManagerBuilding = GetComponent<EventManagerBuilding>();
    }

    protected virtual void Start()
    {
        ChangeState(Constants.STATE_PLACING);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
