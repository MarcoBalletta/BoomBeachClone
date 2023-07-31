using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventManager))]
public class GameManager : Singleton<GameManager>
{
    private EventManager eventManager;

    public EventManager EventManager { get => eventManager; }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManager>();
    }
}
