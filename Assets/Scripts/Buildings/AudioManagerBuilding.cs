using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class AudioManagerBuilding : AudioManager<Building>
{
    [SerializeField] private AudioClip placedClip;

    protected override void OnEnable()
    {
        base.OnEnable();
        controller.EventManager.onPlacedBuilding += PlacedBuildingClip;
    }

    private void PlacedBuildingClip()
    {
        PlayOneShotAudio(placedClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        controller.EventManager.onPlacedBuilding -= PlacedBuildingClip;
    }


}
