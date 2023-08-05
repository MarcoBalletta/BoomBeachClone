using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mine : Building, IClickable, IPointerClickHandler
{

    [SerializeField] private float explosionRange;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask layerMaskExplosion;

    public void ClickedOn()
    {
        Explode();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedOn();
    }

    private void Explode()
    {
        var hitEnemies = Physics.OverlapSphere(transform.position, explosionRange, layerMaskExplosion, QueryTriggerInteraction.Ignore);
        foreach(var enemy in hitEnemies)
        {
            if(enemy.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(damage);
            }
        }
    }
}
