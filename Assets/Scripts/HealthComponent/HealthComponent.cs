using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthComponent : MonoBehaviour, IDamageable
{

    private float hp;
    private float actualHP;
    [SerializeField] private Slider healthBarSlider;

    protected void SetupHealthComponent(DamageableData data)
    {
        hp = data.resistance;
        actualHP = hp;
        if(healthBarSlider)
            UpdateUI();
    }

    public virtual void Damage(float damage)
    {
        actualHP -= damage;
        UpdateUI();
        if (actualHP <= 0) 
            Dead();
    }

    private void UpdateUI()
    {
        healthBarSlider.value = actualHP * 100 / hp;
    }

    protected virtual void Dead(){ }
}
