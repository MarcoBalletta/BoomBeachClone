using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(StateManagerEnemy))]
[RequireComponent(typeof(EventManagerEnemy))]
public class Enemy : MonoBehaviour
{
    //private NavMeshAgent agent;
    private StateManagerEnemy stateManager;
    private EventManagerEnemy eventManager;
    private Building targetBuilding;
    [SerializeField] private EnemyData data;

    public EventManagerEnemy EventManager { get => eventManager; }
    public Building TargetBuilding { get => targetBuilding; }
    public EnemyData Data { get => data; set => data = value; }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        eventManager.onResearchEnded += SetTargetBuilding;
        eventManager.onMovementEnded += StartAttacking;
        eventManager.onAttackEnded += StartResearch;
    }

    private void Start()
    {
        SetupData();
        StartResearch();
    }

    private void StartResearch()
    {
        stateManager.ChangeState(Constants.STATE_RESEARCH);
    }

    private void SetTargetBuilding(Building building)
    {
        targetBuilding = building;
        stateManager.ChangeState(Constants.STATE_MOVEMENT);
    }

    private void Init()
    {
        eventManager = GetComponent<EventManagerEnemy>();
        stateManager = GetComponent<StateManagerEnemy>();
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
