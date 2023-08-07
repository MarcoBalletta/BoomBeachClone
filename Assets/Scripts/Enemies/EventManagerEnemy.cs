using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerEnemy : EventManager
{

    public delegate void OnSetupEnemy(EnemyData data);
    public OnSetupEnemy onSetupEnemy;

    public delegate void OnResearchStarted();
    public OnResearchStarted onResearchStarted;

    public delegate void OnResearchEnded(Defense defense);
    public OnResearchEnded onResearchEnded;

    public delegate void OnFoundBuilding();
    public OnResearchEnded onFoundBuilding;

    public delegate void OnMovementStarted();
    public OnMovementStarted onMovementStarted;

    public delegate void OnMovementEnded();
    public OnMovementEnded onMovementEnded;

    public delegate void OnAttackStarted();
    public OnAttackStarted onAttackStarted;

    public delegate void OnAttackEnded();
    public OnAttackEnded onAttackEnded;

    public delegate void OnDead(Enemy enemy);
    public OnDead onDead;

    public delegate void OnHit(Vector3 position);
    public OnHit onHit;

    public delegate void OnShoot(Transform transform);
    public OnShoot onShoot;

    public delegate void OnEndGame(bool result);
    public OnEndGame onEndGame;
}
