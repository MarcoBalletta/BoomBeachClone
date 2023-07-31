using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateEnemy", menuName = "CreateData/Create new enemy")]
public class EnemyCreator : ScriptableObject
{
    public float speed;
    public float resistance;
    public float firePower;
    public float attackFrequency;
}
