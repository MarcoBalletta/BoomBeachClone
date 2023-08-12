using UnityEngine;
using UnityEngine.UI;

public abstract class HealthComponent : MonoBehaviour, IDamageable
{

    protected float hp;
    protected float actualHP;
    [SerializeField] protected Slider healthBarSlider;

    //setups the UI and value of health
    protected void SetupHealthComponent(DamageableData data)
    {
        hp = data.resistance;
        actualHP = hp;
        if(healthBarSlider)
            UpdateUI();
    }

    protected void ShowHealthUI()
    {
        healthBarSlider.gameObject.SetActive(true);
    }

    protected void HideHealthUI()
    {
        healthBarSlider.gameObject.SetActive(false);
    }

    //updates health value and UI, if dead called dead function 
    public virtual void Damage(float damage, Vector3 position)
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
