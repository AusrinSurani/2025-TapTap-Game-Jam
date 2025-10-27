using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCircleDisplay : MonoBehaviour
{
    public float rotateSpeed = 90f;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation *= Quaternion.Euler(0f, 0, -10f* rotateSpeed*Time.deltaTime) ;
    }
}
