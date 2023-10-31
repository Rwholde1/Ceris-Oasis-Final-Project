using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{

    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // Rotation speed in degrees per second

    private void Start()
    {
        transform.rotation = Quaternion.identity;

    }
    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }


}


