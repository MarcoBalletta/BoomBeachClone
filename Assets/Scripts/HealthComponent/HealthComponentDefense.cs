using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponentDefense : HealthComponent
{
    private Defense defense;

    void Awake()
    {
        defense = GetComponent<Defense>();
    }

    private void OnEnable()
    {
        defense.EventManager.onSetupBuilding += SetupHealthComponent;
    }

    private void OnDisable()
    {
        defense.EventManager.onSetupBuilding -= SetupHealthComponent;
    }

    public override void Damage(float damage)
    {
        if(defense.EventManager.onHit != null)
            defense.EventManager.onHit();
        base.Damage(damage);
    }

    protected override void Dead()
    {
        base.Dead();
        Debug.Log("Dead defense");
        defense.EventManager.onDead(defense);
        Destroy(gameObject, 0.1f);
    }
}
