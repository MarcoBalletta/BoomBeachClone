using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI buildingsValueText;
    [SerializeField] private TextMeshProUGUI enemiesAValueText;
    [SerializeField] private TextMeshProUGUI enemiesBValueText;
    [SerializeField] private TextMeshProUGUI enemiesCValueText;

    private void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        quitButton.onClick.AddListener(() => QuitGame());
    }

    private void StartGame()
    {
        SceneManager.LoadScene(Constants.COMBAT_SCENE_NAME);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void SetBuildingsValue()
    {
        buildingsValueText.text = DataForGameHandler.instance.DataGame.numberOfPlaceableBuildings.ToString();
    }

    public void SetEnemiesAValue()
    {
        enemiesAValueText.text = DataForGameHandler.instance.DataGame.enemiesA.ToString();
    }

    public void SetEnemiesBValue()
    {
        enemiesBValueText.text = DataForGameHandler.instance.DataGame.enemiesB.ToString();
    }

    public void SetEnemiesCValue()
    {
        enemiesCValueText.text = DataForGameHandler.instance.DataGame.enemiesC.ToString();
    }
}
