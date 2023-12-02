using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    public int totalgrenades = 0;
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
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<EMPGrenade>().player = gameObject;
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce);
        totalgrenades--;
    }

    public void getmoreGrenades(int newgrenades)
    {
        totalgrenades += newgrenades;
    }
    }
