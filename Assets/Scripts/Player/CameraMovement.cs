using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//handles the camera movement, with mobile and pc inputs
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

    //handle of the double click for panning
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

    //timer handling for the second click in time
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

    //hanldes the click releasing, for the double click panning
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

    //checks the movement of the finger, if the camera can pan rotates it in the chosen direction
    private void PanCamera()
    {
        if (isPanning)
        {
            var mouseDifference = (input.PlayerInput.MousePosition.ReadValue<Vector2>().x - lastPositionMouse.x);
            Quaternion rotation = pivot.transform.rotation;
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y + panSpeed * mouseDifference, rotation.eulerAngles.z);
            pivot.rotation = rotation;
        }
    }

    private void Zoom()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Mathf.Clamp(GetZoomValue(), -1,1), minZoomValue, maxZoomValue);
    }

    //gets the zoom value
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

    //gets the pinch value and direction
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
        return zoomPinchSpeed * mult;
    }
    #endregion
}
