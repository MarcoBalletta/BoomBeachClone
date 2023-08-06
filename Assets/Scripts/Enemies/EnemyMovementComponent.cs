using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementComponent : MonoBehaviour
{

    private Enemy enemyController;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private float rangeToAttack;
    private float speed;

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
    }

    private void OnDisable()
    {
        enemyController.EventManager.onMovementStarted -= MoveToDestination;
        enemyController.EventManager.onSetupEnemy -= SetupMovementComponent;
    }

    private void SetupMovementComponent(EnemyData data)
    {
        rangeToAttack = data.range;
        speed = data.speed;
        SetSpeedAgent(GameManager.instance.SimulationSpeed);
    }

    private void SetSpeedAgent(int value)
    {
        agent.speed = speed * value;
    }

    private void MoveToDestination()
    {
        SetSpeedAgent(GameManager.instance.SimulationSpeed);
        ResumeAgent();
        agent.destination = enemyController.TargetBuilding.transform.position;
        StartCoroutine(CheckDistance());
    }

    private IEnumerator CheckDistance()
    {
        while(Vector3.Distance(transform.position, agent.destination) >= rangeToAttack)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopAgent();
        //raycast verso target building per cercare ostacoli
        enemyController.EventManager.onMovementEnded();
    }

    private void StopAgent()
    {
        agent.isStopped = true;
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
}
