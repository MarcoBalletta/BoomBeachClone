using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class AudioManagerBuilding : AudioManager<Building>
{
    [SerializeField] private AudioClip placedClip;
    [SerializeField] protected AudioClip deadClip;

    protected override void OnEnable()
    {
        base.OnEnable();
        controller.EventManager.onPlacedBuilding += PlacedBuildingClip;
        controller.EventManager.onDead += DestroyedSound;
    }

    //clip placed building
    private void PlacedBuildingClip()
    {
        PlayOneShotAudio(placedClip);
    }

    //clip destroyed building
    private void DestroyedSound(Building building)
    {
        PlayOneShotAudio(deadClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        controller.EventManager.onPlacedBuilding -= PlacedBuildingClip;
        controller.EventManager.onDead -= DestroyedSound;
    }


}
