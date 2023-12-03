using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{

    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // Rotation speed in degrees per second
    [SerializeField] private int forwardspeed = 0;
    private bool collided = false;
    private void Start()
    {
        transform.rotation = Quaternion.identity;

    }
    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        collided = true;
    }
    public bool hascollided()
    {
        return collided;
    }
}


