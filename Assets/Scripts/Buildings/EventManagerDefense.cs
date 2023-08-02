using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerDefense : EventManagerBuilding
{

    public delegate void OnFoundEnemy(Enemy enemy);
    public OnFoundEnemy onFoundEnemy;

    public delegate void OnEnemyKilled();
    public OnEnemyKilled onEnemyKilled;

    public delegate void OnResearchEnded();
    public OnResearchEnded onResearchEnded;

    public delegate void OnSetupBuilding(DefenseData data);
    public OnSetupBuilding onSetupBuilding;

    public delegate void OnHit();
    public OnHit onHit;

    public delegate void OnDead();
    public OnDead onDead;
}