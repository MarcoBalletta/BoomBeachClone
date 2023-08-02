using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private SphereCollider coll;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private float firePower;
    private Vector3 direction;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(float damage, Vector3 direction)
    {
        firePower = damage;
        this.direction = direction;
    }

    private void Update()
    {
        MoveBullet();
    }

    protected virtual void MoveBullet()
    {
        rb.AddForce(direction * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<Collider>().TryGetComponent(out HealthComponent damagedEntity))
        {
            damagedEntity.Damage(firePower);
        }
        Debug.Log("Hit entity : " + damagedEntity.name);
    }
}
