using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(StateManagerEnemy))]
[RequireComponent(typeof(EventManagerEnemy))]
public class Enemy : Controller
{
    private StateManagerEnemy stateManager;
    private EventManagerEnemy eventManager;
    private Defense targetBuilding;
    [SerializeField] private EnemyData data;

    public EventManagerEnemy EventManager { get => eventManager; }
    public Defense TargetBuilding { get => targetBuilding; }
    public EnemyData Data { get => data; set => data = value; }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        eventManager.onResearchEnded += SetTargetDefense;
        eventManager.onMovementEnded += StartAttacking;
        eventManager.onAttackEnded += StartResearch;
    }

    private void OnDisable()
    {
        eventManager.onResearchEnded -= SetTargetDefense;
        eventManager.onMovementEnded -= StartAttacking;
        eventManager.onAttackEnded -= StartResearch;
    }

    private void Start()
    {
        SetupData();
        StartResearch();
    }

    //change state to research
    private void StartResearch()
    {
        stateManager.ChangeState(Constants.STATE_RESEARCH);
    }

    //sets target to defense
    private void SetTargetDefense(Defense building)
    {
        targetBuilding = building;
        targetBuilding.EventManager.onDead += TargetDead;
        stateManager.ChangeState(Constants.STATE_MOVEMENT);
    }

    //initialise data
    private void Init()
    {
        eventManager = GetComponent<EventManagerEnemy>();
        stateManager = GetComponent<StateManagerEnemy>();
    }

    //if target dead attack ended
    private void TargetDead(Building defense)
    {
        if(eventManager.onAttackEnded != null)
            eventManager.onAttackEnded();
    }

    private void SetupData()
    {
        eventManager?.onSetupEnemy(data);
    }

    private void StartAttacking()
    {
        stateManager.ChangeState(Constants.STATE_ATTACK);
    }
}
