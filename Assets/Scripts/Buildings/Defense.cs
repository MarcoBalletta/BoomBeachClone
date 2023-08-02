using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManagerDefense))]
[RequireComponent(typeof(EventManagerDefense))]
public class Defense : Building
{

    [SerializeField] private DefenseData data;
    private List<Enemy> targets = new List<Enemy>();
    private new StateManagerDefense stateManager;
    private new EventManagerDefense eventManager;

    public new EventManagerDefense EventManager { get => eventManager; set => eventManager = value; }
    public List<Enemy> Targets { get => targets; set => targets = value; }

    protected override void Awake()
    {
        base.Awake();
        stateManager = GetComponent<StateManagerDefense>();
        eventManager = GetComponent<EventManagerDefense>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventManager.onEnemyKilled += TargetKilled;
        eventManager.onFoundEnemy += AddEnemyToList;
    }

    private void Start()
    {
        eventManager.onSetupBuilding(data);
    }

    public void AddEnemyToList(Enemy enemy)
    {
        if (!targets.Contains(enemy))
            targets.Add(enemy);
    }

    public Enemy GetTarget()
    {
        if (targets.Count == 0) return null;
        return targets[0];
    }

    public void TargetKilled()
    {

    }

    //public void Setup()
    //{
    //    coll.radius = data.range;
    //}
}
