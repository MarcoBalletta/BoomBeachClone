using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacingUI : MonoBehaviour
{
    private Building building;
    [SerializeField] private GameObject border;
    [SerializeField] private Button placingButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button deselectButton;

    private void Awake()
    {
        building = GetComponentInParent<Building>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placingButton.onClick.AddListener(() => PlaceButtonPressed());
        deselectButton.onClick.AddListener(() => DeselectButtonPressed());
        rotateButton.onClick.AddListener(() => RotateButtonPressed());
    }

    private void OnEnable()
    {
        //GameManager.instance.EventManager.onBuildingModeStarted += ShowUI;
        building.EventManager.onPlacedBuilding += HideUI;
        building.EventManager.onBuildingModeActivated += ShowUI;
        //building.EventManager.onPlacedBuilding += PlacedBuilding;
    }

    private void OnDisable()
    {
        //GameManager.instance.EventManager.onBuildingModeStarted -= ShowUI;
        building.EventManager.onBuildingModeActivated -= ShowUI;
        building.EventManager.onPlacedBuilding -= HideUI;
        //building.EventManager.onPlacedBuilding -= PlacedBuilding;
    }

    private void PlaceButtonPressed()
    {
        building.EventManager.onBuildingModeReleased();
    }

    private void DeselectButtonPressed()
    {
        GameManager.instance.EventManager.onBuildingDeselectButtonClick();
    }

    private void RotateButtonPressed()
    {
        StartCoroutine(CoroutineRotation());
    }

    private IEnumerator CoroutineRotation()
    {
        border.SetActive(false);
        yield return building.RotateLerpBuilding();
        border.SetActive(true);
    }

    private void HideUI()
    {
        border.SetActive(false);
    }

    //private void PlacedBuilding()
    //{
    //    GameManager.instance.EventManager.onBuildingModeStarted -= ShowUI;
    //}

    private void ShowUI()
    {
        border.SetActive(true);
    }
}
