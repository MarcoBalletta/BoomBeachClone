using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingButtonsPanel;
    [SerializeField] private Button simulationModeButton;
    [SerializeField] private Button speedUpButton;

    private void Awake()
    {
        simulationModeButton.onClick.AddListener(() => GameManager.instance.PlayModeActivated());
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onPlacingModeStarted += ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted += ShowPlayButton;
        GameManager.instance.EventManager.onPlacingModeEnded += DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded += HidePlayButton;
    }

    private void OnDisable()
    {
        GameManager.instance.EventManager.onPlacingModeStarted -= ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded -= DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted -= ShowPlayButton;
        GameManager.instance.EventManager.onPlacingModeEnded -= HidePlayButton;
    }

    #region Building buttons panel

    private void DeactivateBuildingButtonsPanel()
    {
        buildingButtonsPanel.SetActive(false);
    }

    private void ActivateBuildingButtonsPanel()
    {
        buildingButtonsPanel.SetActive(true);
    }

    #endregion

    private void HidePlayButton()
    {
        simulationModeButton.gameObject.SetActive(false);
    }

    private void ShowPlayButton()
    {
        simulationModeButton.gameObject.SetActive(true);
    }
}
