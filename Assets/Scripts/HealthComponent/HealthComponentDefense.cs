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

    //calls hit event
    public override void Damage(float damage, Vector3 position)
    {
        if(defense.EventManager.onHit != null)
            defense.EventManager.onHit(position);
        base.Damage(damage, position);
    }

    //calls dead event
    protected override void Dead()
    {
        base.Dead();
        defense.EventManager.onDeadDefense(defense);
        defense.EventManager.onDead(defense);
        Destroy(gameObject, 0.5f);
    }
}
