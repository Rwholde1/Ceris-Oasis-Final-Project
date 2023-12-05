using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AkaneExample : NetworkBehaviour
{

    public NetworkObject boomer;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void throwBoomer() {

        //GameObject clone = Instantiate(boomer, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation) as GameObject;
        Vector3 throwPoint = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        createBoomerServerRpc(throwPoint, transform.rotation, LobbySceneManagement.singleton.getLocalPlayer().identity);

    }


    [ServerRpc(RequireOwnership = false)]
    void createBoomerServerRpc(Vector3 throwPoint, Quaternion throwDir, int playerID) {
        Debug.Log("Threw boomer serverrpc");
        NetworkObject rang = Instantiate(boomer, throwPoint, Quaternion.identity);
        rang.transform.rotation = throwDir;
        rang.GetComponent<NetworkObject>().Spawn();
        //rang.GetComponent<Rigidbody>().AddForce(throwDir);
        rang.GetComponent<Boomerang>().pID = playerID;
    }
}