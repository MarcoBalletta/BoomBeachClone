using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyAttackComponent : MonoBehaviour
{

    private Enemy enemyController;
    [SerializeField] private float rotationSpeed;
    private float attackRate;
    private float firePower;
    private Bullet bulletPrefab;
    private Coroutine shootRoutine;
    [SerializeField] private Transform bulletSpawn;

    void Awake()
    {
        enemyController = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemyController.EventManager.onAttackStarted += AttackStarted;
        enemyController.EventManager.onSetupEnemy += SetupAttackComponent;
        enemyController.EventManager.onDead += DeathEnemy;
    }

    private void OnDisable()
    {
        enemyController.EventManager.onAttackStarted -= AttackStarted;
        enemyController.EventManager.onSetupEnemy -= SetupAttackComponent;
        enemyController.EventManager.onDead -= DeathEnemy;
    }

    private void SetupAttackComponent(EnemyData data)
    {
        attackRate = data.attackFrequency;
        firePower = data.firePower;
        bulletPrefab = data.projectile;
    }

    private void AttackStarted()
    {
        enemyController.TargetBuilding.EventManager.onDead += TargetDefenseDead;
        shootRoutine = StartCoroutine(ShootCoroutine());
    }

    private IEnumerator RotateTowardsTargetBuilding()
    {
        Vector3 direction = ((enemyController.TargetBuilding.transform.position + Vector3.up) - transform.position).normalized;
        Quaternion lookGoal = Quaternion.LookRotation(direction);
        while(Quaternion.Angle(transform.rotation, lookGoal) > 6)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookGoal, rotationSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void ImmediateRotateToTarget()
    {
        transform.LookAt(enemyController.TargetBuilding.transform);
    }

    private IEnumerator ShootCoroutine()
    {
        //yield return RotateTowardsTargetBuilding();
        ImmediateRotateToTarget();
        while (enemyController.TargetBuilding != null)
        {
            Shoot();
            yield return new WaitForSeconds(attackRate / GameManager.instance.SimulationSpeed);
        }
    }
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        if(enemyController.EventManager.onShoot != null)
            enemyController.EventManager.onShoot(bulletSpawn);
        bullet.Setup(firePower, transform.forward.normalized, false);
    }

    private void TargetDefenseDead(Defense defense)
    {
        if(shootRoutine != null) StopCoroutine(shootRoutine);
        enemyController.EventManager.onAttackEnded();
    }

    private void DeathEnemy(Enemy enemy)
    {
        if(enemyController.TargetBuilding != null)
        {
            enemyController.TargetBuilding.EventManager.onDead -= TargetDefenseDead;
        }
    }

    #region Version with Task
    private Task RotateTowardsBuilding()
    {
        Vector3 direction = ((enemyController.TargetBuilding.transform.position + Vector3.up) - transform.position).normalized;
        Quaternion lookGoal = Quaternion.LookRotation(direction);
        while (Quaternion.Angle(transform.rotation, lookGoal) > 0.1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookGoal, rotationSpeed);
            Task.Delay((int)Time.deltaTime * 1000);
        }
        return Task.CompletedTask;
    }

    private async void ShootTask()
    {
        await RotateTowardsBuilding();
        while(enemyController.TargetBuilding != null)
        {
            Shoot();
            await Task.Delay((int)enemyController.Data.attackFrequency * 1000);
        }
        enemyController.EventManager.onAttackEnded();
    }
    #endregion
}
