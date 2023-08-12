using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<PoolerEntity> poolers = new List<PoolerEntity>();
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    private float wallRadius;
    private float ringRadius;

    private void OnEnable()
    {
        GameManager.instance.EventManager.onSimulationModeStarted += SpawnEnemies;
    }

    private void Awake()
    {
        wallRadius = (outerRadius - innerRadius) * 0.5f;
        ringRadius = wallRadius + innerRadius;
        transform.position = GameManager.instance.GetCenterGrid();
    }

    //adds the pooler object to list
    public void SubscribePoolerToList(PoolerEntity pooler)
    {
        if(!poolers.Contains(pooler))
            poolers.Add(pooler);
    }

    private void SpawnEnemies()
    {
        SpawnEnemiesTask();
    }

    //spawns the enemies in the pooler
    private async void SpawnEnemiesTask()
    {
        foreach(var pooler in poolers)
        {
            await SpawnAllEnemiesInPool(pooler);
        }
    }

    private Task SpawnAllEnemiesInPool(PoolerEntity pooler)
    {
        int numberOfElements = pooler.EntityList.Count;
        for (int i = 0; i< numberOfElements; i++)
        {
            var entity = pooler.GetEntity();
            entity.transform.position = GetRandomPositionInsideTorus();
            entity.gameObject.SetActive(true);
            Task.Delay(200);
        }
        return Task.CompletedTask;
    }

    //gets random position in torus 
    private Vector3 GetRandomPositionInsideTorus()
    {
        float rndAngle = Random.value * 6.28f; // use radians, saves converting degrees to radians

        // determine position
        float cX = Mathf.Sin(rndAngle);
        float cZ = Mathf.Cos(rndAngle);

        Vector3 ringPos = new Vector3(cX, 0, cZ);
        ringPos *= ringRadius;

        Vector3 sPos = Random.insideUnitSphere * wallRadius;

        return (ringPos + sPos) + transform.position;
    }
}
