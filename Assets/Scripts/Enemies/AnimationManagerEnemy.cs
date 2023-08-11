using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagerEnemy : AnimationManager<Enemy> 
{

    private EventManagerEnemy eventManager;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerEnemy>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void StartWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, true);
    }

    private void StopWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, false);
    }

    private void Death()
    {
        animator.SetTrigger(Constants.ANIMATION_DEATH);
    }

    private void Shoot()
    {
        animator.SetTrigger(Constants.ANIMATION_SHOOT);
    }
}
