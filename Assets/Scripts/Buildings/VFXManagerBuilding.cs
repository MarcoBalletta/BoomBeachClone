using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManagerBuilding : VFXManager<Building>
{
    [SerializeField] protected GameObject vfxPlaced;
    [SerializeField] protected GameObject deadVFX;


    protected override void OnEnable()
    {
        controller.EventManager.onPlacedBuilding += PlacedBuildingVFX;
        controller.EventManager.onDead += DestroyedVFX;
    }

    private void PlacedBuildingVFX()
    {
        var vfx = Instantiate(vfxPlaced, transform.position, Quaternion.identity);
    }

    private void DestroyedVFX(Building building)
    {
        var vfx = Instantiate(deadVFX, transform.position, Quaternion.identity);
    }

    protected override void OnDisable()
    {
        controller.EventManager.onPlacedBuilding -= PlacedBuildingVFX;
        controller.EventManager.onDead = DestroyedVFX;
    }
}
