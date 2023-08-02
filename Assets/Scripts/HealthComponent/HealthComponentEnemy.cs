using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponentEnemy : HealthComponent
{

    private EventManagerEnemy eventController;

    // Start is called before the first frame update
    void Awake()
    {
        eventController = GetComponent<EventManagerEnemy>();
    }

    private void OnEnable()
    {
        eventController.onSetupEnemy += SetupHealthComponent;
    }

    protected override void Dead()
    {
        base.Dead();
        eventController.onDead();
    }
}
