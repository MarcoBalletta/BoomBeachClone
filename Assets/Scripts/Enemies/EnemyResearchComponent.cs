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

    private void ResearchBuildingToAttack()
    {
        Building closestBuilding = null;
        float minimumDistance = 0;
        foreach(var building in GameManager.instance.Buildings)
        {
            if (closestBuilding == null)
            {
                closestBuilding = building;
                minimumDistance = Vector3.Distance(transform.position, building.transform.position);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);
                if (distance < minimumDistance)
                {
                    closestBuilding = building;
                    minimumDistance = distance;
                }
            }
        }

        enemyController.EventManager.onResearchEnded(closestBuilding);
    }
}
