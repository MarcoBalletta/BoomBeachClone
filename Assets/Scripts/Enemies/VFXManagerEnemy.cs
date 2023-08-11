using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManagerEnemy : VFXManager<Enemy>
{
    [SerializeField] protected GameObject shootVFX;
    [SerializeField] protected GameObject deadVFX;
    [SerializeField] protected GameObject spawnVFX;

    protected override void OnEnable()
    {
        base.OnEnable();
        controller.EventManager.onDead += DestroyedVFX;
        controller.EventManager.onShoot += ShootVFX;
        controller.EventManager.onSetupEnemy += SpawnVFX;
    }


    protected override void OnDisable()
    {
        base.OnEnable();
        controller.EventManager.onSetupEnemy -= SpawnVFX;
        controller.EventManager.onDead = DestroyedVFX;
        controller.EventManager.onShoot -= ShootVFX;
    }

    private void SpawnVFX(EnemyData data)
    {
        var vfx = Instantiate(spawnVFX, transform.position, transform.rotation);
    }

    private void ShootVFX(Transform transform)
    {
        var vfx = Instantiate(shootVFX, transform.position, transform.rotation);
    }

    private void DestroyedVFX(Enemy enemy)
    {
        var vfx = Instantiate(deadVFX, transform.position, transform.rotation);
    }
}
