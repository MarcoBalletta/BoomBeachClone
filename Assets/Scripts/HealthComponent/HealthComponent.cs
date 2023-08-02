using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthComponent : MonoBehaviour, IDamageable
{

    private float hp;

    protected void SetupHealthComponent(DamageableData data)
    {
        hp = data.resistance;
    }

    public virtual void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0) 
            Dead();
    }

    protected virtual void Dead(){ }
}
