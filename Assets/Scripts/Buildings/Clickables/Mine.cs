using UnityEngine;
using UnityEngine.EventSystems;

//clickable building, explodes on click and does ranged damage to enemies, the layer mask defines the damaged entities
public class Mine : Building, IClickable, IPointerClickHandler
{

    [SerializeField] private float explosionRange;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask layerMaskExplosion;

    //if simulation mode explodes
    public void ClickedOn()
    {
        //if simulation mode
        if(GameManager.instance.IsGameStarted())
            Explode();
    }

    //if clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedOn();
    }

    //finds hit enemies in range
    private void Explode()
    {
        var hitEnemies = Physics.OverlapSphere(transform.position, explosionRange, layerMaskExplosion, QueryTriggerInteraction.Ignore);
        foreach(var enemy in hitEnemies)
        {
            if(enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(damage, enemy.transform.position);
            }
        }
        eventManager.onDead(this);
        Destroy(gameObject, 0.1f);
    }

    //on destroy recalculates nav mesh
    private void OnDestroy()
    {
        GameManager.instance.EventManager.onDestroyableDestroyed();
    }
}
