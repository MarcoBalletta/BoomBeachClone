using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManagerDefense : VFXManagerBuilding
{
    [SerializeField] protected GameObject shootVFX;

    protected Defense Controller { get => (controller as Defense); }

    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.EventManager.onShoot += ShootVFX;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        Controller.EventManager.onShoot -= ShootVFX;
    }

    //spawns the shoot vfx
    private void ShootVFX(Transform transform)
    {
        var vfx = Instantiate(shootVFX, transform.position, transform.rotation);
    }
}
