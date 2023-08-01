using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{

    PoolerEntity pooler;

    public void Setup(PoolerEntity pooler)
    {
        this.pooler = pooler;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        pooler.DisposeEntity(this);
    }
}