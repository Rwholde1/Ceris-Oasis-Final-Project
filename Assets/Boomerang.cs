using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float boomerangSpeed = 10f;
    public float returnTime = 5f;

    private bool isReturning = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Save the initial position and rotation for returning
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isReturning)
        {
            ThrowBoomerang();
        }

        if (isReturning)
        {
            ReturnBoomerang();
        }
    }

    void ThrowBoomerang()
    {
        // Set the boomerang in motion
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * boomerangSpeed;

        // Set a timer to return the boomerang after a specified time
        Invoke("StartReturning", returnTime);
    }

    void ReturnBoomerang()
    {
        // Calculate the distance to move the boomerang back
        float step = boomerangSpeed * Time.deltaTime;

        // Move the boomerang back to the initial position
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

        // Rotate the boomerang back to the initial rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, step);

        // Check if the boomerang has returned to the initial position
        if (transform.position == initialPosition && transform.rotation == initialRotation)
        {
            // Reset velocity and set the returning flag to false
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            isReturning = false;
        }
    }

    void StartReturning()
    {
        // Set the returning flag to true
        isReturning = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the boomerang collides with something other than the player
        if (collision.gameObject.tag != "Player")
        {
            // Start returning the boomerang immediately upon collision
            StartReturning();
        }
    }
}
