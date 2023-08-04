using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuilding : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Building building;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.instance.StateManager.CurrentState.nameOfState == Constants.STATE_PLACING)
            GameManager.instance.EventManager.onBuildingButtonClick(building);
    }
}
