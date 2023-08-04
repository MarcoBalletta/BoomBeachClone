using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class PoolableEnemy : PoolableObject
{

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.instance.EventManager.onSpawnEnemy(enemy);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
