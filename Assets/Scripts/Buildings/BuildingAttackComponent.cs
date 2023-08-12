using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuildingAttackComponent : MonoBehaviour
{
    private Defense defense;
    private Enemy target;
    private SphereCollider coll;
    private Bullet bulletPrefab;
    private float firePower;
    private float attackRate;
    private Coroutine shootingCoroutine;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private bool archTrajectory;

    void Awake()
    {
        defense = GetComponentInParent<Defense>();
        coll = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        defense.EventManager.onSetupBuilding += SetupAttackComponent;
        defense.EventManager.onFoundEnemy += AttackTarget;
        defense.EventManager.onLostEnemy += LostEnemyFromRange;
        defense.EventManager.onDeadDefense += DeathDefense;
    }

    private void OnDisable()
    {
        defense.EventManager.onSetupBuilding -= SetupAttackComponent;
        defense.EventManager.onFoundEnemy -= AttackTarget;
        defense.EventManager.onDeadDefense -= DeathDefense;
    }

    //takes the data from the scriptable object
    private void SetupAttackComponent(DefenseData data)
    {
        coll.radius = data.range;
        bulletPrefab = data.projectile;
        attackRate = data.shotFrequency;
        firePower = data.firePower;
    }

    //starts attacking the target
    private void AttackTarget(Enemy enemy)
    {
        //await Task.Delay(50);
        if (defense.Targets.Count == 0 || target != null) return;
        target = enemy;
        target.EventManager.onDead += TargetLost;
        shootingCoroutine = StartCoroutine(ShootCoroutine());
    }

    //coroutine to shoot the target and wait attack rate
    private IEnumerator ShootCoroutine()
    {
        while (target != null)
        {
            SetRotationBulletSpawn();
            Shoot();
            yield return new WaitForSeconds(attackRate / GameManager.instance.SimulationSpeed);
        }
    }

    //lost target, searches next
    private void TargetLost(Enemy enemy)
    {
        StopCoroutine(shootingCoroutine);
        target = null;
        SearchNextTarget();
    }

    //enemy out of range, if target lost target
    private void LostEnemyFromRange(Enemy enemy)
    {
        if (enemy == target) 
        {
            TargetLost(enemy);
        } 
    }

    //shoot the bullet to target, setup bullet
    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        if(defense.EventManager.onShoot != null)
            defense.EventManager.onShoot(bulletSpawn);
        bullet.Setup(firePower, bulletSpawn.transform.forward.normalized, archTrajectory);
    }

    //if arch trajectory do arch, else do straight trajectory
    private void SetRotationBulletSpawn()
    {
        if (archTrajectory)
        {
            //do trajectory

            //Quaternion angleRotation = Quaternion.LookRotation(target.transform.position + target.transform.up - transform.position, transform.up);
            
            //angleRotation.x = RotateBulletSpawn();
            //Vector3 euler = angleRotation.eulerAngles;
            //euler.x = RotateBulletSpawn();
            ////angleRotation.eulerAngles = euler;
            //bulletSpawn.rotation = angleRotation;
            bulletSpawn.rotation = Quaternion.LookRotation(target.transform.position + target.transform.up - bulletSpawn.transform.position, transform.up);
        }
        else
        {
            bulletSpawn.rotation = Quaternion.LookRotation(target.transform.position + target.transform.up - bulletSpawn.transform.position, transform.up);
        }
    }

    //private float RotateBulletSpawn()
    //{
    //    float speed = bulletPrefab.Speed;
    //    float deltaX = Mathf.Abs(bulletSpawn.position.x - target.transform.position.x);
    //    float deltaY = bulletSpawn.position.y - target.transform.position.y;
    //    float gravity = - Physics.gravity.y;
    //    float gravityperxsquare = gravity * Mathf.Pow(deltaX, 2);
    //    float velocitySquare = Mathf.Pow(speed, 2);
    //    float firstCalculateAngle = ( gravityperxsquare/ velocitySquare) - deltaY;
    //    float squareHeight = Mathf.Pow(deltaY, 2);
    //    float squareHorizontal = Mathf.Pow(deltaX, 2);
    //    float root = Mathf.Sqrt(squareHeight + squareHorizontal);
    //    float angleWithoutPhaseAngle =Mathf.Rad2Deg * Mathf.Acos(( Mathf.Deg2Rad * (firstCalculateAngle/ root)));
    //    float phaseAngle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Deg2Rad *( deltaX / deltaY));
    //    float angle = (angleWithoutPhaseAngle + phaseAngle) / 2;
    //    return angle;
    //}

    //searches next target from list of enemies attackable
    protected virtual void SearchNextTarget()
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

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            defense.EventManager.onLostEnemy(enemy);
        }
    }

    //if defenses is destroyed unsubscribe itself from target death event
    private void DeathDefense(Defense defense)
    {
        if (target != null)
        {
            target.EventManager.onDead -= TargetLost;
        }
    }
}
