using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;

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

        Vector3 throwpoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z + 0.1f);
        //GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
                GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);

        grenade.GetComponent<MolotovCocktail>().player = gameObject;
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        /*
        Ray ray = LobbySceneManagement.singleton.playerCamObject.ScreenPointToRay(Input.mousePosition);
        Debug.Log("call ping");
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            Debug.Log("naed thrown");
            grenade.GetComponent<Transform>().LookAt(hit.transform, Vector3.up);
        }*/
        rb.velocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(transform.forward * throwForce);

        /*
        if (transform.forward.y >= 0) {
            rb.AddForce(Vector3.Scale(transform.forward, new Vector3(throwForce, throwForce, throwForce)));
            Debug.Log("threw up");
        } else {
            rb.AddForce(Vector3.Scale(transform.forward, new Vector3(throwForce, 3 * throwForce, throwForce)));
            Debug.Log("threw down");
        }*/
    }

    }
