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
        defense.EventManager.onBuildingModeActivated += HideHealthUI;
        defense.EventManager.onPlacedBuilding += ShowHealthUI;
    }

    private void OnDisable()
    {
        defense.EventManager.onSetupBuilding -= SetupHealthComponent;
        defense.EventManager.onBuildingModeActivated -= HideHealthUI;
        defense.EventManager.onPlacedBuilding -= ShowHealthUI;
    }

    public override void Damage(float damage, Vector3 position)
    {
        if(defense.EventManager.onHit != null)
            defense.EventManager.onHit(position);
        base.Damage(damage, position);
    }

    protected override void Dead()
    {
        base.Dead();
        Destroy(gameObject, 0.1f);
        defense.EventManager.onDead(defense);
    }
}
