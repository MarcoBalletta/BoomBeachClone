using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManagerEnemy))]
public class StateManagerEnemy : StateManager
{

    private EventManagerEnemy eventManager;

    public EventManagerEnemy EventManager { get => eventManager; }

    protected override void SetupStates()
    {
        listOfStates.Add(Constants.STATE_MOVEMENT, new StateMovement(this));
        listOfStates.Add(Constants.STATE_RESEARCH, new StateResearch(this));
        listOfStates.Add(Constants.STATE_ATTACK, new StateAttack(this));
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerEnemy>();
    }

    private void Start()
    {
        ChangeState(Constants.STATE_RESEARCH);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
