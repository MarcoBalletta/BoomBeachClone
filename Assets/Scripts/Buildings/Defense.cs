using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManagerDefense))]
[RequireComponent(typeof(EventManagerDefense))]
public class Defense : Building
{

    [SerializeField] protected DefenseData data;
    protected List<Enemy> targets = new List<Enemy>();
    protected new StateManagerDefense stateManager;
    protected new EventManagerDefense eventManager;

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

    protected override void OnDisable()
    {
        eventManager.onEnemyKilled -= TargetKilled;
        eventManager.onFoundEnemy -= AddEnemyToList;
    }

    private void Start()
    {
        eventManager.onSetupBuilding(data);
    }

    public void AddEnemyToList(Enemy enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy);
            enemy.EventManager.onDead += TargetKilled;
        }
    }

    public virtual Enemy GetTarget()
    {
        if (targets.Count == 0) return null;
        return targets[0];
    }

    public void TargetKilled(Enemy enemy)
    {
        targets.Remove(enemy);
        targets.TrimExcess();
    }

    //public void Setup()
    //{
    //    coll.radius = data.range;
    //}
}
