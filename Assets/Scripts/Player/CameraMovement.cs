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
    [SerializeField] private float zoomPinchSpeed;
    private bool canMoveCamera = true;
    private bool isPanning = false;
    private bool firstClick = false;
    private float actualTimerSecondClick;
    private Coroutine secondTimerCoroutine;
    [SerializeField] private Vector3 cameraOffsetFromPivot;
    [SerializeField] private Transform pivot;
    private Vector2 lastPositionMouse;
    private bool isSecondaryTouch = false;
    private float distance;
    private float previousDistance = 0;

    private void Awake()
    {
        input = new InputPlayer();
        input.Enable();
#if UNITY_ANDROID
        zoomPinchSpeed *= 10;
#endif
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onDraggingBuilding += CannotMoveCamera;
        GameManager.instance.EventManager.onStopDraggingBuilding += CanMoveCamera;
        input.PlayerInput.MouseClick.started += PressMouse;
        input.PlayerInput.MouseClick.canceled += ReleaseMouse;
        input.PlayerInput.SecondaryTouchContact.started += SecondaryTouchPressed;
        input.PlayerInput.SecondaryTouchContact.canceled += SecondaryTouchReleased;
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
        lastPositionMouse = input.PlayerInput.MousePosition.ReadValue<Vector2>();
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
            //var mouseDifference = (Mouse.current.position.ReadValue().x - lastPositionMouse.x);
            var mouseDifference = (input.PlayerInput.MousePosition.ReadValue<Vector2>().x - lastPositionMouse.x);
            Quaternion rotation = pivot.transform.rotation;
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y + panSpeed * mouseDifference, rotation.eulerAngles.z);
            pivot.rotation = rotation;
        }
    }

    private void Zoom()
    {
        //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Mathf.Clamp(input.PlayerInput.Zoom.ReadValue<Vector2>().y, -1,1), minZoomValue, maxZoomValue);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Mathf.Clamp(GetZoomValue(), -1,1), minZoomValue, maxZoomValue);
        Debug.Log("GetZoomValue: " + GetZoomValue() + "FieldofView: " + Camera.main.fieldOfView);
    }

    private float GetZoomValue()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            //touch device
            if (isSecondaryTouch)
            {
                return GetTouchPinchValue();
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return input.PlayerInput.Zoom.ReadValue<Vector2>().y;
        }
    }

    private void AlignCameraWithCenterOfGrid()
    {
        pivot.transform.position = GameManager.instance.GetCenterGrid();
        transform.position = cameraOffsetFromPivot;
        transform.LookAt(pivot.transform.position);
    }

    #region Touchscreen
    private void SecondaryTouchReleased(InputAction.CallbackContext obj)
    {
        isSecondaryTouch = false;
    }

    private void SecondaryTouchPressed(InputAction.CallbackContext obj)
    {
        isSecondaryTouch = true;
        distance = 0;
    }

    private float GetTouchPinchValue()
    {
        distance = Vector2.Distance(input.PlayerInput.MousePosition.ReadValue<Vector2>(), input.PlayerInput.SecondaryFingerPosition.ReadValue<Vector2>());
        float mult = 0;
        //zoom out
        if(distance > previousDistance)
        {
            mult = 1;
        }
        //zoom in
        else if(distance < previousDistance)
        {
            mult = -1;
        }
        previousDistance = distance;
        Debug.Log("Pinch speed:" + zoomPinchSpeed * mult);
        return zoomPinchSpeed * mult;
    }

    #endregion
}
