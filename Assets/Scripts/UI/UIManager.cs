using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingButtonsPanel;
    [SerializeField] private GameObject speedUpPanel;
    [SerializeField] private Button simulationModeButton;
    [SerializeField] private Button speedUpButton;
    [SerializeField] private TextMeshProUGUI speedUpValue;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;

    private void Awake()
    {
        simulationModeButton.onClick.AddListener(() => GameManager.instance.PlayModeActivated());
        retryButton.onClick.AddListener(() => GameManager.instance.ReloadScene());
        speedUpButton.onClick.AddListener(() => GameManager.instance.ToggleSpeedUpButton());
        backToMenuButton.onClick.AddListener(() => GameManager.instance.BackToMenu());
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onPlacingModeStarted += ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted += HideSpeedUpPanel;
        GameManager.instance.EventManager.onPlacingModeStarted += ShowPlayButton;
        GameManager.instance.EventManager.onSpeedUpToggle += SetSpeedVelocityValue;
        GameManager.instance.EventManager.onPlacingModeEnded += DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded += HidePlayButton;
        GameManager.instance.EventManager.onSimulationModeStarted += ShowSpeedUpPanel;
        GameManager.instance.EventManager.onSimulationModeEnded += HideSpeedUpPanel;
        GameManager.instance.EventManager.onEndMatch += EndGame;
    }



    private void OnDisable()
    {
        GameManager.instance.EventManager.onSpeedUpToggle -= SetSpeedVelocityValue;
        GameManager.instance.EventManager.onPlacingModeStarted -= ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded -= DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted -= ShowPlayButton;
        GameManager.instance.EventManager.onPlacingModeEnded -= HidePlayButton;
        GameManager.instance.EventManager.onSimulationModeStarted -= ShowSpeedUpPanel;
        GameManager.instance.EventManager.onEndMatch -= EndGame;
    }

    #region Building buttons panel

    private void DeactivateBuildingButtonsPanel()
    {
        buildingButtonsPanel.SetActive(false);
    }

    private void ActivateBuildingButtonsPanel()
    {
        if(GameManager.instance.CanPlaceOtherBuildings())
            buildingButtonsPanel.SetActive(true);
    }

    #endregion

    #region PlayButton
    private void HidePlayButton()
    {
        simulationModeButton.gameObject.SetActive(false);
    }

    private void ShowPlayButton()
    {
        simulationModeButton.gameObject.SetActive(true);
    }
    #endregion

    #region SpeedUpPanel
    private void HideSpeedUpPanel()
    {
        speedUpPanel.SetActive(false);
    }

    private void ShowSpeedUpPanel()
    {
        speedUpPanel.SetActive(true);
    }
    #endregion

    private void SetSpeedVelocityValue(int value)
    {
        speedUpValue.text = value.ToString();
    }

    private void EndGame(bool result)
    {
        HideSpeedUpPanel();
        if (result)
        {
            endGameText.text = Constants.WIN_TEXT;
        }
        else
        {
            endGameText.text = Constants.LOSE_TEXT;
        }
        endGamePanel.SetActive(true);
    }
}
