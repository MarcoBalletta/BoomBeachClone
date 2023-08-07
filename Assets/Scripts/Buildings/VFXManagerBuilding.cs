using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManagerBuilding : VFXManager<Building>
{
    protected GameObject vfxPlaced;

    protected override void OnEnable()
    {
        controller.EventManager.onPlacedBuilding += PlacedBuildingVFX;
    }

    private void PlacedBuildingVFX()
    {
        var vfx = Instantiate(vfxPlaced, transform.position, Quaternion.identity);
    }

    protected override void OnDisable()
    {
        controller.EventManager.onPlacedBuilding -= PlacedBuildingVFX;
    }
}
