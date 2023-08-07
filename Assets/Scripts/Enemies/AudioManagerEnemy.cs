using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerEnemy : AudioManager<Enemy>
{
    [SerializeField] protected AudioClip shootClip;
    [SerializeField] protected AudioClip deadClip;

    protected override void OnEnable()
    {
        base.OnEnable();
        controller.EventManager.onDead += DestroyedSound;
        controller.EventManager.onShoot += ShootSound;
    }

    private void ShootSound(Transform transform)
    {
        PlayOneShotAudio(shootClip);
    }

    private void DestroyedSound(Enemy enemy)
    {
        PlayOneShotAudio(deadClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        controller.EventManager.onDead -= DestroyedSound;
        controller.EventManager.onShoot -= ShootSound;
    }
}
