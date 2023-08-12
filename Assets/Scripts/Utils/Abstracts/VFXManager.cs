using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VFXManager<T> : MonoBehaviour where T: Controller
{

    protected T controller;

    protected virtual void Awake()
    {
        controller = GetComponent<T>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
}
