using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagerEnemy : AnimationManager<Enemy> 
{

    private EventManagerEnemy eventManager;

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerEnemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
