using UnityEngine;

public class HealthComponentEnemy : HealthComponent
{

    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemy.EventManager.onSetupEnemy += SetupHealthComponent;
    }

    private void OnDisable()
    {
        enemy.EventManager.onSetupEnemy -= SetupHealthComponent;
    }

    //damage and calls on hit event
    public override void Damage(float damage, Vector3 position)
    {
        if (enemy.EventManager.onHit != null)
            enemy.EventManager.onHit(position);
        base.Damage(damage, position);
    }

    //calls on dead event
    protected override void Dead()
    {
        base.Dead();
        enemy.EventManager.onDead(enemy);
        Destroy(gameObject, 2f);
    }
}
