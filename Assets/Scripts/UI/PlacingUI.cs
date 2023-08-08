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

    void Start()
    {
        placingButton.onClick.AddListener(() => PlaceButtonPressed());
        deselectButton.onClick.AddListener(() => DeselectButtonPressed());
        rotateButton.onClick.AddListener(() => RotateButtonPressed());
    }

    private void OnEnable()
    {
        building.EventManager.onPlacedBuilding += HideUI;
        building.EventManager.onBuildingModeActivated += ShowUI;
    }

    private void OnDisable()
    {
        building.EventManager.onBuildingModeActivated -= ShowUI;
        building.EventManager.onPlacedBuilding -= HideUI;
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

    private void ShowUI()
    {
        border.SetActive(true);
    }
}
