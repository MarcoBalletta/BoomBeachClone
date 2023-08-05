using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyResearchComponent : MonoBehaviour
{
    private Enemy enemyController;

    private void Awake()
    {
        enemyController = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemyController.EventManager.onResearchStarted += ResearchBuildingToAttack;
    }

    private void OnDisable()
    {
        enemyController.EventManager.onResearchStarted -= ResearchBuildingToAttack;
    }

    private void ResearchBuildingToAttack()
    {
        Defense closestDefense = null;
        float minimumDistance = 0;
        if (GameManager.instance.Defenses.Count == 0) return;
        foreach(var defense in GameManager.instance.Defenses)
        {
            if (closestDefense == null)
            {
                closestDefense = defense;
                minimumDistance = Vector3.Distance(transform.position, defense.transform.position);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, defense.transform.position);
                if (distance < minimumDistance)
                {
                    closestDefense = defense;
                    minimumDistance = distance;
                }
            }
        }
        enemyController.EventManager.onResearchEnded(closestDefense);
    }
}
