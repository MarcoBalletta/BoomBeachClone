using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataForGameHandler : PersistentSingleton<DataForGameHandler>
{
    private DataGame dataGame = new DataGame();

    public DataGame DataGame { get => dataGame; }

    public void SetNumberOfBuildings(Slider value)
    {
        dataGame.numberOfPlaceableBuildings = (int)value.value;
    }

    //sets number of enemiesA, at least one enemy has to be at 1
    public void SetNumberOfEnemiesA(Slider value)
    {
        if (value.value == 0 && dataGame.enemiesB == 0 && dataGame.enemiesC == 0) 
        {
            value.value = dataGame.enemiesA;
            return;
        } 
        dataGame.enemiesA = (int)value.value;
    }

    //sets number of enemiesB, at least one enemy has to be at 1
    public void SetNumberOfEnemiesB(Slider value)
    {
        if (value.value == 0 && dataGame.enemiesA == 0 && dataGame.enemiesC == 0) 
        {
            value.value = dataGame.enemiesB;
            return;
        } 
        dataGame.enemiesB = (int)value.value;
    }

    //sets number of enemiesC, at least one enemy has to be at 1
    public void SetNumberOfEnemiesC(Slider value)
    {
        if (value.value == 0 && dataGame.enemiesA == 0 && dataGame.enemiesB == 0) 
        {
            value.value = dataGame.enemiesC;
            return;
        } 
        dataGame.enemiesC = (int)value.value;
    }
}
