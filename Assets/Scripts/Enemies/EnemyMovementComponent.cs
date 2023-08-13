using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//enemy movement component, after found defense target moves to reach it
//TO DO insert A* movement
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementComponent : MonoBehaviour
{

    private Enemy enemyController;
    private NavMeshAgent agent;
    [SerializeField] private LayerMask layerMaskCheckClearView;
    private Rigidbody rb;
    private float rangeToAttack;
    private float speed;
    private Coroutine movementCoroutine;

    void Awake()
    {
        enemyController = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onSpeedUpToggle += SetSpeedAgent;
        enemyController.EventManager.onMovementStarted += MoveToDestination;
        enemyController.EventManager.onSetupEnemy += SetupMovementComponent;
        enemyController.EventManager.onDead += DeadEnemy;
    }

    private void OnDisable()
    {
        GameManager.instance.EventManager.onSpeedUpToggle -= SetSpeedAgent;
        enemyController.EventManager.onMovementStarted -= MoveToDestination;
        enemyController.EventManager.onSetupEnemy -= SetupMovementComponent;
        enemyController.EventManager.onDead -= DeadEnemy;
    }

    //setups movement component
    private void SetupMovementComponent(EnemyData data)
    {
        rangeToAttack = data.range;
        speed = data.speed;
        SetSpeedAgent(GameManager.instance.SimulationSpeed);
    }

    //agent speed based on data and simulation speed
    private void SetSpeedAgent(int value)
    {
        agent.speed = speed * value;
    }

    //reaches destination based on range and visibility of target
    private void MoveToDestination()
    {
        SetSpeedAgent(GameManager.instance.SimulationSpeed);
        ResumeAgent();
        agent.destination = enemyController.TargetBuilding.transform.position;
        if (movementCoroutine != null) 
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        } 
        movementCoroutine = StartCoroutine(CheckDistance());
    }

    //checks if enemy has clear view to target 
    private IEnumerator CheckDistance()
    {
        bool clearViewToTarget = false;
        Debug.Log("Target: " + enemyController.TargetBuilding + " --- Range: " + rangeToAttack + " ---Distance: " + Vector3.Distance(transform.position, agent.destination) + "Clear view: " + clearViewToTarget); ;
        while(Vector3.Distance(transform.position, agent.destination) >= rangeToAttack || !clearViewToTarget)
        {
            Debug.Log("Cycle");
            if(Physics.Raycast(transform.position, (agent.destination - transform.position).normalized,  out RaycastHit hit, rangeToAttack, layerMaskCheckClearView, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Hit: " + hit.collider.name);
                if(hit.collider.TryGetComponent(out Building building) && building == enemyController.TargetBuilding)
                {
                    clearViewToTarget = true;
                }
                clearViewToTarget = false;
            }
            else
            {
                clearViewToTarget = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopAgent();
        //raycast verso target building per cercare ostacoli
        enemyController.EventManager.onMovementEnded();
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;
    }

    //stops the agent movement
    private void StopAgent()
    {
        if(agent.isActiveAndEnabled && !agent.isStopped) agent.isStopped = true;
        rb.freezeRotation = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
    }

    private void ResumeAgent()
    {
        agent.enabled = true;
        rb.freezeRotation = false;
        agent.isStopped = false;
        agent.speed = speed;
    }

    private void DeadEnemy(Enemy enemy)
    {
        StopAgent();
        StopAllCoroutines();
    }
}
