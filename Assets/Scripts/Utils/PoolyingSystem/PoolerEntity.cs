using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolerEntity : MonoBehaviour
{
    [Min(1)]
    [SerializeField] private uint initialSpawnNumber;
    [SerializeField] private PoolableObject entityToSpawn;
    private List<PoolableObject> entityList = new List<PoolableObject>();

    public List<PoolableObject> EntityList { get => entityList; }

    protected virtual void Start()
    {
        InitialSpawnEntities();
    }

    private void InitialSpawnEntities()
    {
        for (int i = 0; i < initialSpawnNumber; i++)
        {
            var entity = Instantiate(entityToSpawn, transform);
            entity.Setup(this);
            entity.gameObject.SetActive(false);
        }
    }

    public PoolableObject GetEntity()
    {
        PoolableObject entityPassed;
        if (entityList.Count > 0) entityPassed = entityList.First(x => !x.gameObject.activeInHierarchy);
        else entityPassed = GetNewEntity();
        entityList.Remove(entityPassed);
        return entityPassed;
    }

    private PoolableObject GetNewEntity()
    {
        var entity = Instantiate(entityToSpawn, transform);
        entity.Setup(this);
        entityList.Add(entity);
        return entity;
    }

    public void DisposeEntity(PoolableObject entity)
    {
        entity.transform.SetParent(transform);
        entity.gameObject.SetActive(false);
        if (!entityList.Contains(entity)) entityList.Add(entity);
    }
}