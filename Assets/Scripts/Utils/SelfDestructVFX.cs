using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructVFX : MonoBehaviour
{

    private ParticleSystem vfx;

    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, vfx.main.duration);   
    }
}
