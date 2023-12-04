using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkaneExample : MonoBehaviour
{

    public GameObject boomer;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject clone;
            clone = Instantiate(boomer, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
        }
    }
}