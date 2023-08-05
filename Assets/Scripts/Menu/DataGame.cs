using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGame
{
    [Range(1,10)]
    public int numberOfPlaceableBuildings = 1;
    [Range(0,10)]
    public int enemiesA = 1;
    [Range(0, 10)]
    public int enemiesB = 1;
    [Range(0, 10)]
    public int enemiesC = 1;


    public DataGame() 
    { 
        
    }
}
