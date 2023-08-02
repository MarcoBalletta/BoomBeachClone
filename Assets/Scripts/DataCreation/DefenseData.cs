using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CreateDefense", menuName ="CreateData/Create new defense")]
public class DefenseData : DamageableData
{
    public Bullet projectile;
    public float range;
    public float firePower;
    public float shotFrequency;
}
