using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
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
        Vector3 throwpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<MolotovCocktail>().player = gameObject;
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce);
    }

    }
