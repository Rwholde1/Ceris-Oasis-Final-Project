using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MolotovThrower : NetworkBehaviour
{
    public float throwForce = 40f;
    public NetworkObject grenadePrefab;

    //
    public float sensitivity = 2f;
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;
    //

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {

        /*
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        //transform.eulerAngles = new Vector3(-velocity.y, velocity.x, 0f);
        Vector3 dir = new Vector3(-velocity.y, velocity.x, 0f);

        */

        Vector3 throwpoint = new Vector3(transform.position.x, transform.position.y - 0f, transform.position.z + 0.1f);
        createFireGrenadeServerRpc(throwpoint, transform.forward * throwForce);
        Debug.Log("Threw molotov");
        //GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
                //GameObject grenade = Instantiate((GameObject) grenadePrefab, throwpoint, Quaternion.identity);

        // grenade.GetComponent<MolotovCocktail>().player = gameObject;
        // Rigidbody rb = grenade.GetComponent<Rigidbody>();
        /*
        Ray ray = LobbySceneManagement.singleton.playerCamObject.ScreenPointToRay(Input.mousePosition);
        Debug.Log("call ping");
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            Debug.Log("naed thrown");
            grenade.GetComponent<Transform>().LookAt(hit.transform, Vector3.up);
        }*/
        // rb.velocity = new Vector3(0f, 0f, 0f);
                // rb.AddForce(transform.forward * throwForce);

        /*
        if (transform.forward.y >= 0) {
            rb.AddForce(Vector3.Scale(transform.forward, new Vector3(throwForce, throwForce, throwForce)));
            Debug.Log("threw up");
        } else {
            rb.AddForce(Vector3.Scale(transform.forward, new Vector3(throwForce, 3 * throwForce, throwForce)));
            Debug.Log("threw down");
        }*/
    }

    [ServerRpc(RequireOwnership = false)]
    void createFireGrenadeServerRpc(Vector3 throwPoint, Vector3 throwDir) {
        Debug.Log("Threw molotov serverrpc");
        NetworkObject molotov = Instantiate(grenadePrefab, throwPoint, Quaternion.identity);
        molotov.GetComponent<NetworkObject>().Spawn();
        molotov.GetComponent<Rigidbody>().AddForce(throwDir);
    }

    /*
    public void createPing() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Debug.Log("call ping");
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            Debug.Log("ping");
            //Sets height offset and instantiates ping
            Vector3 offset = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
            NetworkObject ping = Instantiate(basicPing, offset, Quaternion.identity);
            ping.GetComponent<NetworkObject>().Spawn();
            //ping.SpawnWithOwnership(OwnerClientId);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    void createPingServerRpc(Vector3 offset) {
        NetworkObject ping = Instantiate(basicPing, offset, Quaternion.identity);
        ping.GetComponent<NetworkObject>().Spawn();
    }*/

}
