using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    private List<PoolerEntity> poolers = new List<PoolerEntity>();
    private BoxCollider coll;

    private void Start()
    {
        GameManager.instance.EventManager.onSimulationModeStarted += SpawnEnemies;
        coll = GetComponent<BoxCollider>();
    }

    public void SubscribePoolerToList(PoolerEntity pooler)
    {
        if(!poolers.Contains(pooler))
            poolers.Add(pooler);
    }

    private void SpawnEnemies()
    {
        SpawnEnemiesTask();
    }

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
            entity.transform.position = GetRandomPositionInsideCollider();
            entity.gameObject.SetActive(true);
            Task.Delay(200);
        }
        return Task.CompletedTask;
    }

    private Vector3 GetRandomPositionInsideCollider()
    {
        float posX = Random.Range(coll.bounds.min.x, coll.bounds.max.x);
        float posZ = Random.Range(coll.bounds.min.z, coll.bounds.max.z);
        return new Vector3(posX, transform.position.y, posZ);
    }
}
