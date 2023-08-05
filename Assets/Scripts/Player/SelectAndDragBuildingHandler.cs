using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectAndDragBuildingHandler : MonoBehaviour
{
    private InputPlayer input;
    private bool clickedOnSelectedBuilding = false;
    private Building selectedBuilding;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask layerMaskBuilding;

    private void Awake()
    {
        input = new InputPlayer();
        input.Enable();
    }

    private void OnEnable()
    {
        input.PlayerInput.MouseClick.performed += MouseClicked;
        input.PlayerInput.MouseClick.canceled += MouseReleased;
        GameManager.instance.EventManager.onBuildingButtonClick += SpawnBuildingSelected;
        GameManager.instance.EventManager.onBuildingClick += SelectBuilding;
        GameManager.instance.EventManager.onBuildingDeselectButtonClick += DeselectBuilding;
    }

    private void OnDisable()
    {
        input.PlayerInput.MouseClick.performed -= MouseClicked;
        input.PlayerInput.MouseClick.canceled -= MouseReleased;
        GameManager.instance.EventManager.onBuildingClick -= SelectBuilding;
        GameManager.instance.EventManager.onBuildingButtonClick -= SpawnBuildingSelected;
        GameManager.instance.EventManager.onBuildingDeselectButtonClick -= DeselectBuilding;
    }

    private void SpawnBuildingSelected(Building building)
    {
        var buildingToSpawn = Instantiate(building, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) , Quaternion.identity);
        SelectBuilding(buildingToSpawn);
        GameManager.instance.EventManager.onBuildingClick(selectedBuilding);
    }

    private void SelectBuilding(Building building)
    {
        selectedBuilding = building;
        clickedOnSelectedBuilding = true;
        building.StartBuildingMode();
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
        if (selectedBuilding != null && clickedOnSelectedBuilding)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 5000f, layerMask, QueryTriggerInteraction.Ignore))
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

    private void DeselectBuilding()
    {
        selectedBuilding?.DeselectedBuilding();
    }

    private void MouseReleased(InputAction.CallbackContext obj)
    {
        if (selectedBuilding)
        {
            //selectedBuilding.EventManager.onBuildingModeReleased();
            clickedOnSelectedBuilding = false;
            GameManager.instance.EventManager.onStopDraggingBuilding();
        }
    }

    private void MouseClicked(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 5000f, layerMaskBuilding, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out Building building))
            {
                if (building == selectedBuilding || selectedBuilding == null)
                {
                    GameManager.instance.EventManager.onBuildingClick(building);
                    GameManager.instance.EventManager.onDraggingBuilding();
                }
            }
        }
    }
}
