using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponentDefense : HealthComponent
{
    private EventManagerDefense eventController;

    void Awake()
    {
        eventController = GetComponent<EventManagerDefense>();
    }

    private void OnEnable()
    {
        eventController.onSetupBuilding += SetupHealthComponent;
    }

    public override void Damage(float damage)
    {
        if(eventController.onHit != null)
            eventController.onHit();
        base.Damage(damage);
    }

    protected override void Dead()
    {
        base.Dead();
        Debug.Log("Dead");
        eventController.onDead();
    }
}
