using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CreateDefense", menuName ="CreateData/Create new defense")]
public class DefenseData : ScriptableObject
{
    public GameObject projectile;
    public float range;
    public float resistance;
    public float firePower;
    public float shotFrequency;
}
