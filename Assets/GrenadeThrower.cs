using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GrenadeThrower : NetworkBehaviour
{
    public float throwForce = 50f;
    public NetworkObject grenadePrefab;
    public int totalgrenades = 2;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && totalgrenades != 0)
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        Vector3 throwpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        createEMPGrenadeServerRpc(throwpoint, transform.forward * throwForce, LobbySceneManagement.singleton.getLocalPlayer().identity);
        //GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        //grenade.GetComponent<EMPGrenade>().player = gameObject;
        //grenade.GetComponent<EMPGrenade>().pID = LobbySceneManagement.singleton.getLocalPlayer().identity;
        /*
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(0f, 0f, 0f);
        rb.AddForce(transform.forward * throwForce);*/
        totalgrenades--;
    }

    [ServerRpc(RequireOwnership = false)]
    void createEMPGrenadeServerRpc(Vector3 throwPoint, Vector3 throwDir, int playerID) {
        Debug.Log("Threw emp serverrpc");
        NetworkObject emp = Instantiate(grenadePrefab, throwPoint, Quaternion.identity);
        emp.GetComponent<NetworkObject>().Spawn();
        emp.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        emp.GetComponent<Rigidbody>().AddForce(throwDir);
        emp.GetComponent<EMPGrenade>().pID = playerID;
    }

    public void getmoreGrenades(int newgrenades)
    {
        totalgrenades += newgrenades;
    }
}
