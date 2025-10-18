using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDuct : MonoBehaviour
{
    public float speed;
    
    private void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * speed);
    }
}
