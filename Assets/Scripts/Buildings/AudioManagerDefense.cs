using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerDefense : AudioManagerBuilding
{
    [SerializeField] protected AudioClip shootClip;

    protected Defense Controller { get => (controller as Defense); }
    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.EventManager.onShoot += ShootSound;
    }

    private void ShootSound(Transform transform)
    {
        PlayOneShotAudio(shootClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.EventManager.onShoot -= ShootSound;
    }
}
