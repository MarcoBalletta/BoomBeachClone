using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private Coroutine shootingCoroutine;
    [SerializeField] private Transform bulletSpawn;

    void Awake()
    {
        defense = GetComponentInParent<Defense>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        defense.EventManager.onSetupBuilding += SetupAttackComponent;
        defense.EventManager.onFoundEnemy += AttackTarget;
        defense.EventManager.onDead += DeathDefense;
    }

    private void OnDisable()
    {
        defense.EventManager.onSetupBuilding -= SetupAttackComponent;
        defense.EventManager.onFoundEnemy -= AttackTarget;
        defense.EventManager.onDead -= DeathDefense;
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
        //await Task.Delay(50);
        if (defense.Targets.Count == 0 || target != null) return;
        target = enemy;
        target.EventManager.onDead += TargetDead;
        shootingCoroutine = StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (target != null)
        {
            Shoot();
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void TargetDead(Enemy enemy)
    {
        StopCoroutine(shootingCoroutine);
        target = null;
        SearchNextTarget();
    }

    private void Shoot()
    {
        Debug.Log("SpawnBullet");
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(target.transform.position + target.transform.up - transform.position, transform.up));
        bullet.Setup(firePower, ((target.transform.position + target.transform.up) - bullet.transform.position));
    }

    private void SearchNextTarget()
    {
        //await Task.Delay((int)Time.deltaTime * 1000);
        if (defense.Targets.Count == 0) return;
        else AttackTarget(defense.GetTarget());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            defense.EventManager.onFoundEnemy(enemy);
        }
    }

    private void DeathDefense(Defense defense)
    {
        if (target != null)
        {
            target.EventManager.onDead -= TargetDead;
        }
    }
}
