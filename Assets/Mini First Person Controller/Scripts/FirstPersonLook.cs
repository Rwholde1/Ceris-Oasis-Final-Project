﻿using UnityEngine;
using Unity.Netcode;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        //character = GetComponentInParent<FirstPersonMovement>().transform;
        //character = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<Transform>();
        //character = NetworkManager.LocalClient.PlayerObject;
        character = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();

    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        //character = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<Transform>();
        //character = NetworkManager.LocalClient.PlayerObject;
        character = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();


    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);


        transform.position = character.transform.position;
        transform.position += new Vector3(0f, 1.488f, 0f);


        // Rotate camera up-down and controller left-right from velocity.
        //Debug.Log("updated mouse look " + velocity.y);

        //transform.rotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        //transform.rotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        transform.eulerAngles = new Vector3(-velocity.y, velocity.x, 0f);

        //character.localRotation = Quaternion.AngleAxis( velocity.y, Vector3.right);

        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        
    }
}
