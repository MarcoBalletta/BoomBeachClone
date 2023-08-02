using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateEnemy", menuName = "CreateData/Create new enemy")]
public class EnemyData : DamageableData
{
    public float speed;
    public Bullet projectile;
    public float range;
    public float firePower;
    public float attackFrequency;
}
