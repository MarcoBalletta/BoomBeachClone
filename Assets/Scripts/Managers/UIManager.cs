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
        GameManager.instance.EventManager.onBuildingModeEnded += DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onBuildingModeEnded += HidePlayButton;
        simulationModeButton.onClick.AddListener(() => GameManager.instance.PlayModeActivated());
    }

    #region Building buttons panel

    private void DeactivateBuildingButtonsPanel()
    {
        buildingButtonsPanel.SetActive(false);
    }

    private void ActivateBuildingButtonsPanel()
    {
        buildingButtonsPanel.SetActive(false);
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
