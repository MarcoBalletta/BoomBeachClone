using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Building : MonoBehaviour
{
    [SerializeField] private DefenseData data;
    private SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    public void Setup()
    {
        coll.radius = data.range;
    }
}
