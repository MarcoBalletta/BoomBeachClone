using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The defense controller
/// Stores the enemies that the defense can hit in a list and attacks one at a time.
/// Initially get the data and setups the defense
/// </summary>
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
    protected DefenseData Data { get => data; }

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

    protected void Start()
    {
        eventManager.onSetupBuilding(data);
    }

    //Adds enemy to list of attackable enemies
    protected virtual void AddEnemyToList(Enemy enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy);
            enemy.EventManager.onDead += TargetKilled;
        }
    }

    //protected virtual void RemoveEnemyFromList(Enemy enemy)
    //{
    //    if (!targets.Contains(enemy))
    //    {
    //        targets.Remove(enemy);
    //        enemy.EventManager.onDead -= TargetKilled;
    //    }
    //}

    //gets the first attackable enemy
    public virtual Enemy GetTarget()
    {
        if (targets.Count == 0) return null;
        return targets[0];
    }

    //removes enemy killed
    public void TargetKilled(Enemy enemy)
    {
        targets.Remove(enemy);
        targets.TrimExcess();
    }
}
