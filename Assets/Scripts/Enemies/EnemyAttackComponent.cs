using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

//the attack enemy component
//TO DO : abstract this class, and create enemy shoot component, derives from attack component, create also an interface with method attack for different approaches to 
//to the attack, ranged or melee
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

    //takes attack data from enemy data
    private void SetupAttackComponent(EnemyData data)
    {
        attackRate = data.attackFrequency;
        firePower = data.firePower;
        bulletPrefab = data.projectile;
    }

    //subscribe to defense death, starts the attack
    private void AttackStarted()
    {
        enemyController.TargetBuilding.EventManager.onDeadDefense += TargetDefenseDead;
        shootRoutine = StartCoroutine(ShootCoroutine());
    }

    //to do if needed slow rotation with dot, like the building rotation
    private void ImmediateRotateToTarget()
    {
        transform.LookAt(enemyController.TargetBuilding.transform);
        bulletSpawn.LookAt(enemyController.TargetBuilding.transform);
    }

    //coroutine to shooting, waits attack rate and then calls the animation to shoot
    private IEnumerator ShootCoroutine()
    {
        //yield return RotateTowardsTargetBuilding();
        while (enemyController.TargetBuilding != null)
        {
            //Shoot();
            ImmediateRotateToTarget();
            enemyController.EventManager.onStartShooting();
            yield return new WaitForSeconds(attackRate / GameManager.instance.SimulationSpeed);
        }
    }

    //spawns bullet and setups it
    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        if(enemyController.EventManager.onShoot != null)
            enemyController.EventManager.onShoot(bulletSpawn);
        bullet.Setup(firePower, bulletSpawn.forward.normalized, false);
    }

    //defense dead
    private void TargetDefenseDead(Defense defense)
    {
        if(shootRoutine != null) StopCoroutine(shootRoutine);
        enemyController.EventManager.onAttackEnded();
    }

    //enemy dead, unsubscribe to on dead defense event
    private void DeathEnemy(Enemy enemy)
    {
        StopAllCoroutines();
        if(enemyController.TargetBuilding != null)
        {
            enemyController.TargetBuilding.EventManager.onDeadDefense -= TargetDefenseDead;
        }
    }
}
