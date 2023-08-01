using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Linq;

public class Pooler<T> : MonoBehaviour where T : PoolableObject
{
    [Min(1)]
    [SerializeField] private uint initialSpawnNumber;
    [SerializeField] private T entityToSpawn;
    private List<T> entityList = new List<T>();

    protected virtual void Start()
    {
        InitialSpawnEntities();
    }

    private void InitialSpawnEntities()
    {
        for (int i = 0; i < initialSpawnNumber; i++)
        {
            var entity = Instantiate(entityToSpawn, transform);
            entity.gameObject.SetActive(false);
        }
    }

    public T GetEntity()
    {
        T entityPassed = entityList.First(x => !x.gameObject.activeInHierarchy);
        if (entityPassed == null) entityPassed = GetNewEntity();
        entityList.Remove(entityPassed);
        return entityPassed;
    }

    private T GetNewEntity()
    {
        var entity = Instantiate(entityToSpawn, transform);
        entityList.Add(entity);
        return entity;
    }

    public void DisposeEntity(T entity)
    {
        entity.gameObject.SetActive(false);
        entity.transform.SetParent(transform);
        entityList.Add(entity);
    }

}