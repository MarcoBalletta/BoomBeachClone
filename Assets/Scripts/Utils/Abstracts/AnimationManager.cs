using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationManager<T> : MonoBehaviour where T : Controller
{
    protected Animator animator;
    protected T controller;

    protected virtual void Awake()
    {
        controller = GetComponentInParent<T>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
}
