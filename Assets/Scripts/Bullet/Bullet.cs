using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    private float firePower;
    private Vector3 direction;
    [SerializeField] private float autoDestructionTime;

    public float Speed { get => speed; set => speed = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, autoDestructionTime);
    }

    public void Setup(float damage, Vector3 direction, bool enableGravity)
    {
        firePower = damage;
        this.direction = direction;
        //useGravity = enableGravity;
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }

    //moves the bullet in the bullet spawn forward direction
    protected virtual void MoveBullet()
    {
        rb.AddForce(direction * speed * GameManager.instance.SimulationSpeed);
    }

    //if hits damageable damages it
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out IDamageable damagedEntity))
        {
            damagedEntity.Damage(firePower, transform.position);
        }
        Destroy(gameObject);
    }
}
