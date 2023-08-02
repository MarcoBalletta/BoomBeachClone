using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyAttackComponent : MonoBehaviour
{

    private Enemy enemyController;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 bulletSpawnPosition;
    [SerializeField] private Bullet bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<Enemy>();
        enemyController.EventManager.onAttackStarted += AttackStarted;
    }

    private void AttackStarted()
    {
        StartCoroutine(ShootCoroutine());
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
            yield return new WaitForSeconds(enemyController.Data.attackFrequency);
        }
        enemyController.EventManager.onAttackEnded();
    }
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
        bullet.Setup(enemyController.Data.firePower, transform.forward);
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
