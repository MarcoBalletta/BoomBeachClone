using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagerEnemy : AnimationManager<Enemy> 
{

    private EventManagerEnemy eventManager;

    protected override void Awake()
    {
        base.Awake();
        eventManager = controller.GetComponent<EventManagerEnemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventManager.onStartShooting += ShootAnimation;
        eventManager.onMovementStarted += StartWalking;
        eventManager.onMovementEnded += StopWalking;
        eventManager.onDead += Death;

        //if there's reload, set speed animation of reload based on attack rate
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventManager.onStartShooting -= ShootAnimation;
        eventManager.onMovementStarted -= StartWalking;
        eventManager.onMovementEnded -= StopWalking;
        eventManager.onDead -= Death;
    }

    private void StartWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, true);
    }

    private void StopWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, false);
    }

    private void Death(Enemy enemy)
    {
        animator.SetTrigger(Constants.ANIMATION_DEATH);
    }

    private void ShootAnimation()
    {
        animator.SetBool(Constants.ANIMATION_SHOOT, true);
    }

    private void StopShooting()
    {
        animator.SetBool(Constants.ANIMATION_SHOOT, false);
    }

    public void MessageShoot()
    {
        SendMessageUpwards("Shoot"); 
    }
}
