using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuilding : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Building building;

    public void ButtonSelected()
    {
        GameManager.instance.EventManager.onBuildingSelected(building);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.EventManager.onBuildingSelected(building);
    }
}
