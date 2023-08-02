using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BuildingAttackComponent : MonoBehaviour
{
    private Defense defense;
    private Enemy target;
    private SphereCollider sphereCollider;
    private Bullet bulletPrefab;
    private float firePower;
    private float attackRate;

    void Awake()
    {
        defense = GetComponent<Defense>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        defense.EventManager.onSetupBuilding += SetupAttackComponent;
        defense.EventManager.onFoundEnemy += AttackTarget;
        defense.EventManager.onEnemyKilled += SearchNextTarget;
    }

    private void SetupAttackComponent(DefenseData data)
    {
        sphereCollider.radius = data.range;
        bulletPrefab = data.projectile;
        attackRate = data.shotFrequency;
        firePower = data.firePower;
    }

    private void AttackTarget(Enemy enemy)
    {
        if (defense.Targets.Count == 0 || target != null) return;
        target = enemy;
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (target != null)
        {
            Shoot();
            yield return new WaitForSeconds(attackRate);
        }
        defense.EventManager.onEnemyKilled();
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, Quaternion.LookRotation(target.transform.position - transform.position, transform.up));
        bullet.Setup(firePower, transform.forward);
    }

    private void SearchNextTarget()
    {
        if (defense.Targets.Count == 0) return;
        else AttackTarget(defense.GetTarget());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            defense.EventManager.onFoundEnemy(enemy);
        }
    }
}
