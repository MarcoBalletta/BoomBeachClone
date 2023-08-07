using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerDefense : AudioManagerBuilding
{
    [SerializeField] protected AudioClip shootClip;
    [SerializeField] protected AudioClip deadClip;

    protected Defense Controller { get => (controller as Defense); }
    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.EventManager.onDead += DestroyedSound;
        Controller.EventManager.onShoot += ShootSound;
    }

    private void ShootSound(Transform transform)
    {
        PlayOneShotAudio(shootClip);
    }

    private void DestroyedSound(Defense defense)
    {
        PlayOneShotAudio(deadClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.EventManager.onDead -= DestroyedSound;
        Controller.EventManager.onShoot -= ShootSound;
    }
}
