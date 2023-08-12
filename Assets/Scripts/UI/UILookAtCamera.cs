using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UILookAtCamera : MonoBehaviour
{

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward, Camera.main.transform.up);
    }
}
