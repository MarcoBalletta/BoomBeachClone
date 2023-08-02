using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragAndDropHandler : MonoBehaviour
{
    private InputPlayer input;
    private Building selectedBuilding;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        input = new InputPlayer();
        input.Enable();
        input.PlayerInput.MouseClick.canceled += MouseReleased;
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onBuildingSelected += SetupBuildingSelected;
    }

    private void SetupBuildingSelected(Building building)
    {
        selectedBuilding = Instantiate(building, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) , Quaternion.identity);
        //selectedBuilding.Setup();
    }

    private void Update()
    {
        if (selectedBuilding == null) return;
        DragBuilding();
    }

    private Vector3 GetWorldClickPosition()
    {
        Vector3 mousePosition = new Vector3(input.PlayerInput.MousePosition.ReadValue<Vector2>().x, input.PlayerInput.MousePosition.ReadValue<Vector2>().y, Camera.main.transform.position.z);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        if (Physics.Raycast(mousePos, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
        {
            return hit.point;
        }
        else
            return mousePos;
    }

    private void DragBuilding()
    {
        if (selectedBuilding != null)
        {
            Vector3 mousePosition = new Vector3(input.PlayerInput.MousePosition.ReadValue<Vector2>().x, input.PlayerInput.MousePosition.ReadValue<Vector2>().y, 0 );
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));
            if (Physics.Raycast(mousePos, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
            {
                selectedBuilding.transform.position = hit.point + Vector3.up * 2;
                if (hit.collider.GetComponent<Tile>())
                {
                    //hit tile, call event
                    if (GameManager.instance.EventManager.onBuildingOnTile != null)
                    {
                        GameManager.instance.EventManager.onBuildingOnTile(selectedBuilding);
                    }
                }
            }
            else
            {
                Vector3 positionMouseWorld = Camera.main.ScreenToWorldPoint(GetWorldClickPosition());
                selectedBuilding.transform.position = positionMouseWorld;
            }
        }
    }

    private void MouseReleased(InputAction.CallbackContext obj)
    {
        //if(selectedBuilding && GameManager.instance.EventManager.onBuildingReleased != null)
        //    GameManager.instance.EventManager.onBuildingReleased(selectedBuilding);
        if (selectedBuilding)
        {
            selectedBuilding.EventManager.onBuildingModeReleased();
            selectedBuilding = null;
        }
    }
}
