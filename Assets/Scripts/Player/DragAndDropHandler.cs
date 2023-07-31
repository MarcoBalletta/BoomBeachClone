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
    private Vector3 mousePosition;
    [SerializeField] private LayerMask layerMask;
    private float zCoordinateMouse;
    //[SerializeField]private float mouseDragSpeed;
    //private Vector3 velocity;

    private void Awake()
    {
        input = new InputPlayer();
        input.Enable();
        input.PlayerInput.MouseClick.performed += MousePressed;
        input.PlayerInput.MouseClick.canceled += MouseReleased;
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onBuildingSelected += SetupBuildingSelected;
    }

    private void SetupBuildingSelected(Building building)
    {
        selectedBuilding = Instantiate(building, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) , Quaternion.identity);
        zCoordinateMouse = Camera.main.WorldToScreenPoint(selectedBuilding.transform.position).z;
        selectedBuilding.Setup();
    }

    private void Update()
    {
        if (selectedBuilding == null) return;
        Debug.Log("Dragging");
        //SetWorldMousePosition();
        DragBuilding();
    }

    private Vector3 SetWorldMousePosition()
    {
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = zCoordinateMouse;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 GetWorldClickPosition()
    {
        Vector3 mousePosition = new Vector3(input.PlayerInput.MousePosition.ReadValue<Vector2>().x, input.PlayerInput.MousePosition.ReadValue<Vector2>().y, Camera.main.transform.position.z);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("MousePosWorld : " + mousePos + "MousePos : " + input.PlayerInput.MousePosition.ReadValue<Vector2>() + "MousePos2: " + Mouse.current.position.ReadValue());
        if (Physics.Raycast(mousePos, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit: " + hit.collider.name);
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
            Debug.Log("MousePosWorld : " + mousePos + "MousePos : " + input.PlayerInput.MousePosition.ReadValue<Vector2>() + "MousePos2: " + mousePosition + "MouseCoord: " + Mouse.current.position.ReadValue());
            Debug.DrawRay(mousePos, Camera.main.transform.forward, Color.black);
            if (Physics.Raycast(mousePos, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
            {
                Debug.Log("Hit raycast");
                selectedBuilding.transform.position = hit.point + Vector3.up * 2;
                if (hit.collider.GetComponent<Tile>())
                {
                    //hit tile, call event
                    if (GameManager.instance.EventManager.onBuildingOnTile != null)
                    {
                        Debug.Log("Hit tile");
                        GameManager.instance.EventManager.onBuildingOnTile(selectedBuilding);
                    }
                }
            }
            else
            {
                Debug.Log("Don't hit");
                Vector3 positionMouseWorld = Camera.main.ScreenToWorldPoint(GetWorldClickPosition());
                //Vector3 newPos = Vector3.SmoothDamp(selectedBuilding.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                selectedBuilding.transform.position = positionMouseWorld;
            }
        }
    }

    private void MousePressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Pressed");
    }

    private void MouseReleased(InputAction.CallbackContext obj)
    {
        Debug.Log("Released");
        if(selectedBuilding && GameManager.instance.EventManager.onBuildingReleased != null)
            GameManager.instance.EventManager.onBuildingReleased(selectedBuilding);
    }
}
