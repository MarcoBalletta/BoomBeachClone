using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    private InputPlayer input;
    [SerializeField] private float zoomValue;
    [SerializeField] private float maxZoomValue;
    [SerializeField] private float minZoomValue;
    [SerializeField] private float panSpeed;
    [SerializeField] private float timeSecondClick;
    private bool canMoveCamera = true;
    private bool isPanning = false;
    private bool firstClick = false;
    private float actualTimerSecondClick;
    private Coroutine secondTimerCoroutine;
    [SerializeField] private Vector3 cameraOffsetFromPivot;
    [SerializeField] private Transform pivot;
    private float direction;
    private Vector2 lastPositionMouse;
    
    private void Awake()
    {
        input = new InputPlayer();
        input.Enable();
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onDraggingBuilding += CannotMoveCamera;
        GameManager.instance.EventManager.onStopDraggingBuilding += CanMoveCamera;
        input.PlayerInput.MouseClick.performed += PressMouse;
        input.PlayerInput.MouseClick.canceled += ReleaseMouse;
    }


    private void OnDisable()
    {
        GameManager.instance.EventManager.onDraggingBuilding -= CannotMoveCamera;
        GameManager.instance.EventManager.onStopDraggingBuilding -= CanMoveCamera;
        input.PlayerInput.MouseClick.performed -= PressMouse;
        input.PlayerInput.MouseClick.canceled -= ReleaseMouse;
    }

    private void Start()
    {
        AlignCameraWithCenterOfGrid();
    }

    private void Update()
    {
        if (!canMoveCamera) return;
        PanCamera();
        Zoom();
        lastPositionMouse = Mouse.current.position.ReadValue();
    }

    private void PressMouse(InputAction.CallbackContext context)
    {
        if(firstClick)
        {
            if (actualTimerSecondClick <= timeSecondClick)
            {
                isPanning = true;
                if (secondTimerCoroutine != null)
                    StopCoroutine(secondTimerCoroutine);
            }
        }
    }

    private IEnumerator StartSecondClickTimer()
    {
        actualTimerSecondClick = 0;
        while(actualTimerSecondClick <= timeSecondClick)
        {
            actualTimerSecondClick += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        firstClick = false;
        if(secondTimerCoroutine != null)    
            StopCoroutine(secondTimerCoroutine);
        secondTimerCoroutine = null;
    }

    private void ReleaseMouse(InputAction.CallbackContext obj)
    {
        if (!firstClick)
        {
            firstClick = true;
            secondTimerCoroutine = StartCoroutine(StartSecondClickTimer());
        }
        else
        {
            firstClick = false;
        }
        isPanning = false;
    }

    private void CanMoveCamera()
    {
        canMoveCamera = true;
    }

    private void CannotMoveCamera()
    {
        canMoveCamera = false;
    }

    private void PanCamera()
    {
        if (isPanning)
        {
            var mouseDifference = (Mouse.current.position.ReadValue().x - lastPositionMouse.x);
            //if ( mouseDifference < 0)
            //    direction = -1;
            //else if (mouseDifference > 0)
            //    direction = 1;
            //else
            //    direction = 0;
            Quaternion rotation = pivot.transform.rotation;
            //rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y + panSpeed * direction * Mathf.Abs(mouseDifference), rotation.eulerAngles.z);
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y + panSpeed * mouseDifference, rotation.eulerAngles.z);
            pivot.rotation = rotation;
        }
    }

    private void Zoom()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Mathf.Clamp(input.PlayerInput.Zoom.ReadValue<Vector2>().y, -1,1), minZoomValue, maxZoomValue);
    }

    private void AlignCameraWithCenterOfGrid()
    {
        pivot.transform.position = GameManager.instance.GetCenterGrid();
        transform.position = cameraOffsetFromPivot;
        transform.LookAt(pivot.transform.position);
    }
}
