using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingButtonsPanel;
    [SerializeField] private Button simulationModeButton;
    [SerializeField] private Button speedUpButton;
    [SerializeField] private TextMeshProUGUI speedUpValue;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private GameObject EndGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;

    private void Awake()
    {
        simulationModeButton.onClick.AddListener(() => GameManager.instance.PlayModeActivated());
        retryButton.onClick.AddListener(() => GameManager.instance.ReloadScene());
        speedUpButton.onClick.AddListener(() => GameManager.instance.ToggleSpeedUpButton());
        //back to menu click add listener
    }

    private void OnEnable()
    {
        GameManager.instance.EventManager.onPlacingModeStarted += ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted += ShowPlayButton;
        GameManager.instance.EventManager.onSpeedUpToggle += SetSpeedVelocityValue;
        GameManager.instance.EventManager.onPlacingModeEnded += DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded += HidePlayButton;
        GameManager.instance.EventManager.onSimulationModeStarted += ShowSpeedUpButton;
        GameManager.instance.EventManager.onSimulationModeEnded += HideSpeedUpButton;
        GameManager.instance.EventManager.onEndMatch += EndGame;
    }



    private void OnDisable()
    {
        GameManager.instance.EventManager.onSpeedUpToggle -= SetSpeedVelocityValue;
        GameManager.instance.EventManager.onPlacingModeStarted -= ActivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeEnded -= DeactivateBuildingButtonsPanel;
        GameManager.instance.EventManager.onPlacingModeStarted -= ShowPlayButton;
        GameManager.instance.EventManager.onPlacingModeEnded -= HidePlayButton;
        GameManager.instance.EventManager.onSimulationModeStarted -= ShowSpeedUpButton;
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

    #region SpeedUpButton
    private void HideSpeedUpButton()
    {
        speedUpButton.gameObject.SetActive(false);
    }

    private void ShowSpeedUpButton()
    {
        speedUpButton.gameObject.SetActive(true);
    }
    #endregion

    private void SetSpeedVelocityValue(int value)
    {
        speedUpValue.text = value.ToString();
    }

    private void EndGame(bool result)
    {
        HideSpeedUpButton();
        if (result)
        {
            endGameText.text = Constants.WIN_TEXT;
        }
        else
        {
            endGameText.text = Constants.LOSE_TEXT;
        }

        EndGamePanel.SetActive(true);
    }
}
