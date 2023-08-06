using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    private List<PoolerEntity> poolers = new List<PoolerEntity>();
    //private MeshCollider coll;
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
        //coll = GetComponentInChildren<MeshCollider>();
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
            entity.transform.position = GetRandomPositionInsideTorus();
            entity.gameObject.SetActive(true);
            Task.Delay(200);
        }
        return Task.CompletedTask;
    }

    //random position in simple mesh
    //private Vector3 GetRandomPositionInsideCollider()
    //{
    //    float posX = Random.Range(coll.bounds.min.x, coll.bounds.max.x);
    //    float posZ = Random.Range(coll.bounds.min.z, coll.bounds.max.z);
    //    return new Vector3(posX, transform.position.y, posZ);
    //}

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
